# amount-in-words
Lightweight REST API which converts currency amount into words

## How to use
To convert currency amount into word http call to the follow endpoint should be performed `https://{host}/amounttoword/?amount={value}`

where `{value}` it is a currency amount which should be converted into words.

`curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET https://{host}/amounttoword?amount=1234.55`

```json
{
    "result": "ќдна тыс€ча двести тридцать четыре рубл€ п€тьдес€т п€ть копеек"
}
```

## Additional query parameters
`convertCents=false` - indicates thet fractional part of amount (cents or copecks) short't be converted

`curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET https://{host}/amounttoword?amount=1234.55&convertCents=false`

```json
{
    "result": "Ўестьдес€т дев€ть тыс€ч двести восемьдес€т четыре рубл€ шестьдес€т семь копеек"
}
```

## Possible errors
Service supports currency amount from 0 up to 999 999 999 999 999.99. If provided amount is bigger than supported or less then zero service will throw an exception with http response code 500
```json
{
    "error": "Unsupported currency amount value"
}

## Live Demo

`curl -i -H "Accept: application/json" -H "Content-Type: application/json" -X GET  https://amount-in-words.azurewebsites.net/amounttoword?amount=984655.99`



