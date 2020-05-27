Feature: Process and retrieve payment
	As a merchant
	I want to process a payment
	So that I can sell a product.

Scenario: Successfully process a payment
	Given the Acquiring Bank is set to respond
		| Field        | Value                            |
		| PaymentId    | AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE |
		| IsSuccessful | true                             |
	When the call to Payment Gateway Api with a new payment is made
		| Field        | Value            |
		| CardNumber   | 4658582263620043 |
		| ExpiryDate   | 0824             |
		| Amount       | 10               |
		| Ccv          | 001              |
		| CurrencyCode | GBP              |
	Then the processed payment is stored
		| Field        | Value                            |
		| CardNumber   | 4658582263620043                 |
		| ExpiryDate   | 0824                             |
		| Amount       | 10                               |
		| CurrencyCode | GBP                              |
		| Ccv          | 001                              |
		| PaymentId    | AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE |
		| IsSuccessful | true                             |
	And the successful payment response is received
		| Field        | Value                            |
		| PaymentId    | AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE |
		| IsSuccessful | true                             |

Scenario: Retrieve the details of a previously made payment
	Given the previous payment is stored
		| Field        | Value                            |
		| CardNumber   | 4658582263620043                 |
		| ExpiryDate   | 0824                             |
		| Amount       | 10                               |
		| CurrencyCode | GBP                              |
		| Ccv          | 001                              |
		| PaymentId    | AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE |
		| IsSuccessful | true                             |
	When the call to Payment Gateway Api with PaymentId 'AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE' is made
	Then the payment details are received
		| Field        | Value                            |
		| CardNumber   | 465858******0043                 |
		| ExpiryDate   | 0824                             |
		| Amount       | 10                               |
		| CurrencyCode | GBP                              |
		| Ccv          | 001                              |
		| PaymentId    | AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE |
		| IsSuccessful | true                             |

Scenario: Receive an error response when retrieving a payment that does not exist
	Given the payment does not exist
	When the call to Payment Gateway Api with PaymentId 'AAAAAAAABBBBCCCCDDDDEEEEEEEEEEEE' is made
	Then not the found result is received

