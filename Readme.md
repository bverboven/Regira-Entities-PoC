# Regira Entities PoC

## Generate data

Run Contoso.Console app to:
- Create SQlite Database
- Add sample entities to DB
- Generate sample attachments

## API

Run Contoso.API app to start Swagger UI.

Sample GET urls:
- https://localhost:7209/courses
- https://localhost:7209/courses/1313
- https://localhost:7209/courses?Q=Fresh%20Towels&PageSize=10
- https://localhost:7209/courses?PageSize=10&Page=2&includes=Instructors&sortBy=Title


## Dependencies

- [Regira Entities](https://github.com/bverboven/Regira-Codebase/tree/master/src/Common.Entities/README.md)
- [EF core](https://github.com/bverboven/Regira-Codebase/tree/master/src/Entities.EFcore)
- [API](https://github.com/bverboven/Regira-Codebase/tree/master/src/Entities.Web)
- [Dependency Injection](https://github.com/bverboven/Regira-Codebase/tree/master/src/Entities.DependencyInjection)
