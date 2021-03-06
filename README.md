# InMemoryStorageService
Simple in-memory key-value store with CRUD operations by REST API
 
|       Operation         |  Method  |                URL                     |                PARAMS                |
|:------------------------|:--------:|:---------------------------------------|:------------------------------------:|
|   Get all rows          |  GET     | `/storage/all`                         | -                                    |
|   Get not empty keys    |  GET     | `/storage/keys`                        | -                                    |
|   Get value by key      |  GET     | `/storage/{key}`                       | -                                    |
|   Set value by key      |  POST    | `/storage/set`                         | `{ "key": string, "value": string }` |
|   Remove value by key   |  DELETE  | `/storage/remove/{key}`                | -                                    |


# Run it
`docker run --rm -e ASPNETCORE_URLS=http://+:5000 -p 5000:5000 navernoe/in-memory-storage-service`


# Task Description

> Реализовать (.net core) доступное по сети in-memory key-value хранилище (сервис).
>
> In-memory означает, что все данные (пары ключ-значение) должны храниться в памяти и при перезапуске хранилища исчезать.
> Требования:  
> - нельзя использовать готовые решения (redis, memcached и тому подобные)  
> - хранилище должно позволять установить значение ключу, прочитать значение ключа, удалить значение ключа, а так же получать список всех ключей, у которых есть значения.  
> - хранилище должно поддерживать несколько одновременных соединений.  
>  
> Проект снабдить тестами.
