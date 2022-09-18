# BookStore

## Assumptions

## Design Notes
-	I don't have ReSharper on my personal PC so code formatting won't be auto checked
-	The amount of abstractions and projects I've done for this project may seem overkill as the project is small but I like to do it to support extending projects and to have things modular
-	I haven't used a proper database implementation as it would take more time to setup. I'd like to use Entity Framework at some point but I haven't yet had the chance to use it properly.
	MySQL could be used but then I'd need to have a local DB setup and I don't feel it offers much more for the tech test

## Process
-	I won't be following any git workflows such as git flow or trunk based as this task is too small for them
-	I've stubbed the project and some interfaces out
-	Next i've began to write tests for the classes and then write the logic for them. I've used a somewhat TDD approach
-	I'm using NUnit for my tests as it's what I'm most familiar with but given more time I'd opt to use XUnit