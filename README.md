# BookStore

Please see the spec.md file in the codebase for the technical spec

## Assumptions

-	It's OK to have a local DB used for the project and use that during run time
-	I've made some rough assumptions on the validation of the book model
-	No acceptance tests are needed

## Running the code

You shouldn't need to do anything specific to run the code. It is worth noting that by default a fresh DB will be used each time, to prevent this change the books.db "Copy to Output directory" property within the CoreDataProvider project.

## Design Notes
-	I've used dependency injection and interfaces to register everything as it provides good flexiblity for testing and if logic/implementations are ever needed to be swapped out.
-	I don't have ReSharper on my personal PC so code formatting won't be auto checked
-	The amount of abstractions and projects I've done for this project may seem overkill as the project is small but I like to do it to support extending projects and to have things modular
-	I've opted to use SqlLite for the database since it was easy to get setup. Ideally I'd like to use EntityFramework which I did try for this but it was taking too long to get setup as I find it quite finicky
-	I've not done any normalisation on the database, ideally there would be a separate Author table which is a ForeignKey to the books table
-	I've created a wrapper around the DB connections that uses Dapper as it's hard to mock Dapper

## Process
-	I won't be following any git workflows such as git flow or trunk based as this task is too small for them
-	I haven't done as many small regular commits as I wanted as i've taken a few breaks throughout coding and the code was rarely in a runnable state

## Testing
-	I TDD'd the application using the red, green, refactor process, however classes were stubbed out prior
-	I've used NUnit as it's what i'm most familiar with so it was the quickest to get setup, however with more time i'd opt for XUnit
-	I've made use of FluentValidations, Moq and AutoFixture for the tests, it might seem a bit overkill but it's my preferred way of doing tests

## What improvements would I make
-	Sadly I don't have ReSharper on my personal PC so could not use that for code clean-up and formatting.   
-	I'd also like to make use of JetBrains' dotCover tool to run code coverage over the solution and see what tests I'm missing. I know I'm missing some sections such as the ConnectionStringConfig class
-	I've used the .net6 new console app template for the program.cs but I can't seem to call it for a test, i'd like investigate this so that I can test that the DI works correctly  
-	I'm not a huge fan of having lots of logic in the controllers so given more time I'd move the validation to the application layer
-   I don't really like having the SQL in the code but it does give benefits of having it in source control, stored procedures may be the better option if they were included in the solution