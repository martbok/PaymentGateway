using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AutoMapper;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Repositories;
using PaymentGateway.Infrastructure.Repositories.Entities;

namespace PaymentGateway.Infrastructure.Repositories
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private readonly IMapper _mapper;
        private readonly ConcurrentDictionary<string, PaymentEntity> _payments = new ConcurrentDictionary<string, PaymentEntity>();

        public InMemoryPaymentRepository(IMapper mapper)
        {
            _mapper = mapper;
            LoadSampleData();
        }

        public void SavePayment(ICompletedPaymentDto newPayment)
        {
            var paymentEntity = _mapper.Map<PaymentEntity>(newPayment);
            if (!_payments.TryAdd(paymentEntity.PaymentId, paymentEntity))
            {
                throw new InvalidOperationException($"Payment '{paymentEntity.PaymentId} already exists in the repository. Cannot overwrite.'");
            }
        }

        public ICompletedPaymentDto GetPayment(string paymentId)
        {
            var paymentEntity = _payments.GetValueOrDefault(paymentId);
            var mapping = _mapper.Map<CompletedPaymentDto>(paymentEntity);
            return mapping;
        }

        private void LoadSampleData()
        {
            _payments.TryAdd("AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE", new PaymentEntity()
            {
                CardNumber = "4658582263620043",
                ExpiryDate = "0824",
                Amount = 10m,
                CurrencyCode = "GBP",
                Ccv = "001",
                PaymentId = "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
                IsSuccessful = true
            });
        }
    }
}