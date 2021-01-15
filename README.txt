WDT Flexible semester assignment 1.
Submitted by Md Abir Ishtiaque, s3677701.

1. Implementation of the two design patterns.

Why have you used the patterns?

Proxy - The proxy classes are responsible for caching data, and validates fields before inserting data.
Adapter - The adapter pattern was used to provide the paging statements method which made use (or adapted) the
        TransactionManagerProxy class.

What advantages do they offer?

Proxy - Since proxy class, caches data it reduces DB calls which results in the program being more performant, and validating fields
        means no bad data gets into the database.
Adapter - Provides a way to get paged results from the database via the manager class without actually changing the manager class.

What would have happened if they were not used?

Proxy - If not used, the database manager classes would have to fetch the data from the DB everytime a user requests a service,
        resulting in poor performance. There would be no validation on nullable fields resulting in corrupt data getting into the DB.
Adapter - In this scenario, the TransactionManagerProxy.cs is a legacy class and is untouched but it can talk to the DB, so without
        the adapter I would have to directly made changes to this piece of 'legacy' code.

How do they make your code elegant?

Proxy - Seperates validation and caching logic.
Adapter - The TransactionManagerProxy class can just be a usual proxy class like the others and not have extra functionality like
          providing paged lists.


2. Justification of class library.
Why have you used it?

The Authentication class library is used to
What advantages does it offer?

Having the auth code in a seperate class library will make it easier to re-use.
How has it made your code elegant?

3. Justification of asynchronous keywords.
Why has this feature been used?
What advantages does it offer?
You need to tell the marker clearly re the location of asynchronous feature in your solution.