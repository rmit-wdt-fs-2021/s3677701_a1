WDT Flexible semester assignment 1.
Submitted by Md Abir Ishtiaque, s3677701.

1. Implementation of the two design patterns.

Why have you used the patterns?

Proxy - The proxy classes are responsible for caching data, and validates fields before inserting data.
Adapter - The adapter class is responsible for providing 

What advantages do they offer?
What would have happened if they were not used?

Proxy - If not used, the database manager classes would have to fetch the data from the DB everytime a user requests a service,
        resulting in poor performance.
Adapter - In this scenario, the TransactionManagerProxy.cs is a legacy class and is untouched but it can talk to the DB, so without
        the adapter I would have to directly made changes to this piece of 'legacy' code.
        
How do they make your code elegant?

2. Justification of class library.
Why have you used it?
What advantages does it offer?
How has it made your code elegant?

3. Justification of asynchronous keywords.
Why has this feature been used?
What advantages does it offer?
You need to tell the marker clearly re the location of asynchronous feature in your solution.