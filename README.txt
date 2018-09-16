Adjust sql connection in appsettings in API and infrastructure project
API should be hosted on 8000 port (http://localhost:8000)

Before you start the project DB migration should be applied 
select infrastructure project in packakeManager console
add-migration Product.Api.Infrastructure.ProductContext 
Update-Database

if you wanna run integration tests then references for ApiClient and ApiContract needs to be added 

If you have some suggestion or want to contact with me you 
can do it by email povilasbarkauskas@gmail.com