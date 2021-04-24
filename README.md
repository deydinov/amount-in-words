# amount-in-words
Lightweight REST API which converts currency amount into words

## How to use
To convert currency amount into word http call to the follow endpoint should be performed `https://{host}/amounttoword/?amount={value}`

where `{value}` it is a currency amount which should be converted into words.

`curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET https://{host}/amounttoword?amount=1234.55`

```json
{
    "result": "Одна тысяча двести тридцать четыре рубля пятьдесят пять копеек"
}
```

## Additional query parameters
`convertCents=false` - indicates that fractional part of amount shouldt't be converted

`curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET https://{host}/amounttoword?amount=984655.99&convertCents=false`

```json
{
    "result": "Девятьсот восемьдесят четыре тысячи шестьсот пятьдесят пять рублей 99 копеек"
}
```

## Possible errors
Service supports amount value between 0 and 999 999 999 999 999.99. If provided amount is bigger than supported or less than zero service will throw an exception with http response code 500
```json
{
    "error": "Unsupported currency amount value"
}

## Live Demo

`curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET  https://amount-in-words.azurewebsites.net/amounttoword?amount=756781034.99`



