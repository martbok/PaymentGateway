## Payment Gateway

# Requirements:
Visual Studio 2019, .Net Core 3.1.
For SpecificationTests 'SpecFlow for Visual Studio 2019' extension is recommended.

In order to have AdapterTest passing, AcquiringBank.Web.Api needs to be running.

# Assumptions
The repository was implemented using in memory collection and processed payments are transient. A sample payment of with Id "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE" was preloaded in the database.

# Sample requests / responses:
GET https://localhost:44322/paymentgatewayapi/v1/payments/AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE
============================================================================
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
----------------------------------------------------------------------------
{
    "cardNumber": "465858******0043",
    "expiryDate": "0824",
    "amount": 10,
    "currencyCode": "GBP",
    "ccv": "001",
    "paymentId": "AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE",
    "isSuccessful": true
}


POST https://localhost:44322/paymentgatewayapi/v1/payments
Content-Type: application/json
----------------------------------------------------------------------------
{
    "cardNumber": "4658582263620043",
    "expiryDate": "0824",
    "amount": 10.50,
    "currencyCode": "GBP",
    "ccv": "001"
}
============================================================================
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Location: https://localhost:44322/paymentgatewayapi/v1/payments/bbab396fa7034cdc861a0dfc23d9220e
----------------------------------------------------------------------------
{
    "paymentId": "bbab396fa7034cdc861a0dfc23d9220e",
    "isSuccessful":true
}


# Solution structure:
Solution follows DDD Hexagonal architecture where we separate Application (currently also Domain), and Infrastructure where dependencies are going from Infrastructure to the Application. Boundaries are isolated using Ports and Adapters.

# Testing:
Outside in TDD approach has been adopted when developing the code. Starring from Acceptance tests written in BDD style using SpecFlow where external dependencies were mocked at the adapter level. Then TDD approach was adopted developing code from outside (first class to receive an external request) to the inside. External dependencies are tested by adapter tests. 

# Discussion for further improvements
Due to time constrains no additional features were developed.

Application logging
For API and services it is essential to have well designed logging. 
Suggested: Azure App Insights and Seriolog.

Application metrics
Apart from logging external dependencies instrumentation should be implemented.
Suggested: Using instrumentation decorators to wrap around adapters.

Containerization
Suggested: Docker and Azure Kubernetes Service (AKS).

Authentication
Azure AD and IP whitelisting are essential for securing API.

API client
It can be created manually or with help OpenApi (Swagger) generators.

Build script / CI
Suggested: Terraform to allow multi-vendor deployment.

Performance testing
Unfortunately, Visual Studio performance testing is being deprecated.
Suggested: JMeter

Encryption
Apart for communication flow through https secure channel, all sensitive data should be always encrypted and decrypted only when needed.

Data storage
SQL or Document database such as Azure CosmosDb should be considered.

Additional considerations
Calling Acquiring Bank Api and Database should have retry policy and idem potency should be considered in order to correctly process payments in case of faults.
