Before you start the project DB migration should be applied 
add-migration Product.Api.Infrastructure.ProductContext 
Update-Database

the project is made by using CQRS methodology also there is introduced like "fake bus" which is dispatching commands and decuples "api ui(controllers)" 
from the core domain. The core business lives in DomainCore also some interfaces like IRepository which is implemented in infrastructure.
Still there is lot of part of code which could be improved. Api client and contract should be published via local nuget
so peoples who wanna use api can take those nugets.
*UI side, display api errors in ui
*api client which should be rewritten in more cleaner way
*swagger for ability so select accept header
*for image in list show only thumbnail like 50x50 and thumbnail should be created on resource creation and saved in separate table 
*write integration tests with api client, unit test for domain commands handlers 
*create patch update for image and product entity fields
*think about ACL for repository so that domain doesn't depend on any technology
*more nicer api global exception handling
*code cleanup

spent time ~13h
still there could be left some bugs so i'll finish on next week. If you have some suggestion or want to contact with me you 
can do it by email povilasbarkauskas@gmail.com