WDT Flexible semester assignment 1.
Submitted by Md Abir Ishtiaque, s3677701.

1. Implementation of the two design patterns.

Why have you used the patterns?

Proxy - The proxy classes are responsible for caching data, and validates fields before inserting data.
Adapter - The adapter pattern was used to provide the paged bank statements method which made use (or adapted) the
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

Class library is used to seperate bits of code that is re-usable in apps. Since all apps require authentication
it made sense to seperate the auth service into a class library.

What advantages does it offer?

If I were to develop a web-api or even a web UI for the banking app I can use this class library for authentication
and so will not have to write the code again.

How has it made your code elegant?

The code is made more elegant as the auth logic is seperate from the other business logic.

3. Justification of asynchronous keywords.

Why has this feature been used?

Using asynchronous programming improves performance bottlenecks and enhances overall responsiveness of the program.

What advantages does it offer?

Async/await is used to make I/O bound functions enable continuous switching between tasks in a single thread instead of 
letting one task blocking the thread.

You need to tell the marker clearly re the location of asynchronous feature in your solution.

I have used the feature when reading the JSON from the web services, when inserting, updating, or
reading data (GetDataTableAsync) from the DB.



Starting code is referenced from Matthew Bolger's TuteLab03.