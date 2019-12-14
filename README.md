# DIFFING API

This is a simple API for differentiating 2 Base64 encoded strings of binary data.

## BASIC INFO

This is a C# API built with ASP.NET. It also contains an SQLite database for persistence.

# HOW IT WORKS

The API consists of 3 endpoints. The `PUT` endpoints expect JSON data in the format:
```json
{ "data": *your Base64 encoded data* }
```
`PUT: /v1/diff/{id}/left`

Sets the left side of the comparison at the provided ID.

`PUT: /v1/diff/{id}/right`

Sets the left side of the comparison at the provided ID.

`GET: v1/diff/{id}`

Responds with the type of diff the API encountered.
If a comparison has not been created at the specified ID,
it responds with code **404**.

If the request ID is valid, then it will return the results in the following format:
```json
{
    "diffResultType": *type of diff*,
    "diffs": [
        {
            "Offset": ...,
            "Length": ...
        },
        ...
    ]
}
```

## TYPES OF DIFFS:
* `Equal` - when both of the strings contain equal data
* `SizeDoNotMatch` - when the length of the 2 strings does not match
* `ContentDoNotMatch` - when the strings are the same length but have differences

**NOTE:** The diffs array field will only be present if the diff type is `ContentDoNotMatch`.
For any other result, the response will only contain the `diffResultType` field.

## REQUIREMENTS:
* Visual Studio 2017
* (optional) DB Browser for SQLite