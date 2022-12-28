# Веб-API с пользовательской проверкой подлинности и удостоверением ASP.NET
---
### В этом проекте реализована аутентификация и авторизация на основе генерации собственных токенов с ограниченным сроком валидности.
---
+   `POST/Auth/login` - После успешной аутентификации клиент в ответ получает JSON в котором содержатся два токена.
    ```
    {
        "accessToken": "2NXt4a5XPiZA5s7uzEi+h1mCg6wGjoMEKPH5KKa1JfkIwgx4QCDylvRSw4Huodz1NjOTjolyopsVPoF4RNXpMOzEJJIWAXwD7R5AKL4L6T0nqwWJKRuWrQ8wyXtwax3xixtfVphUTW9eL7Fns6/2rusaVp4GF7xAsXYm/fjoV+TkIzFVrmlT/02GXAYqg3ZMIbZxf/cOueFyhii4li+yDgew0Ip6KmV94+bEdyrM0BJdRNMJNsuCWeyvPeOkmZAQt6vfXx7YWHmUNRFrSlWFLJiLzaRjVJSh24UfP2gM+dHcM2VQh4NLqw/yYRyLhuxnQTv85nfGNjMx+Vnqz8FERWugapJqC+jCfRS3azyqRu65JGf3mBMiMMn21lzAxISkLB9law3LFizXWDmc0NszxpAgUmv86stteTc0JUjxjco=",
        "refreshToken": "ZdDB0QfQstDz60gbAFq6a/HSHAtwdE5W5dme1/bexbP14EZ40C8wLUoo/nc1H5Id0vMk2EIofCb7hCUXOXKGyw=="
    }
    ```
    Значение токена `accessToken` необходимо использовать при каждом запросе к API `/OpenLoops`. В заголовках запроса с ключём
    ```
    authorization: Bearer 2NXt4a5XPiZA5s7uzEi+h1mCg6wGjoMEKPH5KKa1JfkIwgx4QCDylvRSw4Huodz1V3WKbWq/yN03h8eYPkHysLtE9PoWnPfgGkHXwYreX28p3nmw/Jej4N3NFmfMVoYc3k8j0wxMwZdgHmFNx7DdkRTe1nkdbbTNKeWVgt7XYAYNaHRoTd+aYxZXyekrilupIspd8WLlRkeriqmAQMr8ajotvIHsxq0SvrzQpElxBaCCunB8xS6w+iAtpqlohU/2t+hDCtAAX2w9Q+hRl1Di5q4L8ej27qTFhYf36hupKxL/aGwfT6W4kMxiXw4vQI8c29RhGyd2q5C17EEC2ZMMfJILWNJDBwkGgCVzTiwLbQA9ndBz5KMt70Gm3BtgDyfeIVQuoJFcEBu2uZ9uwhgCQrnePEKdGSOWUQG1BlySmXs=
    ```
---
+   `GET/Auth/tokenValidate` - По этому пути можно проверить валидность текущего токена `accessToken`. Срок валидности токена можно указать в `appsettings.json` - `"token":"accessTokenExpiryMinutes": 5`
---
+   `POST/Auth/register` - Путь авторизации. Принимает строку JSON в котором содержатся ключи и значения для регистрации нового пользователя. 
    ```
    {
        "email": "user@example.com",
        "username": "UserExample",
        "password": "stringst",
        "confirmPassword": "stringst",
        "displayName": "UserExample"
    }
    ```
---
+   `POST/Auth/refresh` - Если срок валидности `accessToken` истёк, а ссесия пользователя продолжается, необходимо обновить токены. По этому пути нужно отправить строку JSON в которой содержаться оба токена. В том же формате в котором они были получены при аутентификации. В ответ будут получены новые пара токенов.
---
+   `POST/Auth/revoke` - Отзыв токена обновления. Отправив по этому пути JSON, в котором должен содержаться только токен `refreshToken` в следующем формате.
    ```
    {
         "refreshToken": "ZdDB0QfQstDz60gbAFq6a/HSHAtwdE5W5dme1/bexbP14EZ40C8wLUoo/nc1H5Id0vMk2EIofCb7hCUXOXKGyw=="
    }
    ```
    Ссесия аутентификации пользователя будет прекращена. Обновление токенов станет не возможным. Для возобновления ссесии требуется вновь пройти аутентификацию. После которой будут получены новая пара токенов.
---

##   OpenLoops
---
+   `GET/OpenLoops` - Запрашиваем все записи из БД

+   `POST/OpenLoops` - Создаём новую запись, отправив JSON в следующем формате.
    ```
    {
        "note": "string",
        "description": "string",
        "CompleteDateUtc": "2022-12-20T13:36:49.992Z",
        "Complete": false
    }
    ```
    `"CompleteDateUtc":` Можно указывать в следующих форматах `"2022-12-20T13:36:49.992Z"` - по умолчанию текущее время, текущего часового пояса. `"2022-12-20T13:36:49.992 +3"` - временная метка часового пояса `+3`.
    А так же можно указать пустую строку `""` в таком случае в БД будет сохранено значениие `-infinity`. Что в свою очередь будет означает, что временная метка отсутствует.

+   `GET/OpenLoops/{id}` - Запрашивает одну запись из БД, с соответствующим `id`

+   `PUT/OpenLoops/{id}` - Вносит изменения в одну запись из БД, с соответствующим `id`. В запросе требуется отправить строку JSON в следующем формате.
    ```
    {
        "note": "Изменить",
        "description": "Изменить Изменить Изменить",
        "CompleteDateUtc": "2022-12-20T04:46:00 +4",
        "Complete": true
    }
    ```
+   `DELETE/OpenLoops/{id}` - Удаляем одну запись из БД, соответствующую `{id}`

![ToDoList.API.jpeg](ToDoList.API.jpeg)
---
![ToDoList.API.gif](ToDoList.API.gif)