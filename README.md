## GraphQL with PrimeNumbers
### Demonstration of GraphQL query, mutation & subscription using GraphQL for .Net
* request post prime sieve with payload amount 10.  (for empty sieve it's from 2 to 12 )
```
mutation {
  postPrimeSieve(
    primeSieve: { amount: 10 }) {
      primeSieveId
      start
      end
    }
}
```
_response:_
``` 
{
  "data": {
    "postPrimeSieve": {
      "primeSieveId": 1, 
      "start": 2, 
      "end": 12
    }
  }
}
```
* request prime number list
```
{
  getPrimeNumberList     
}
```
_response:_
```
{
  "data": {
    "getPrimeNumberList": [2, 3, 5, 7, 11 ]
  }
}
```
* see full demo


https://user-images.githubusercontent.com/14275269/219674356-a76f72bc-dbc8-46b2-ba40-ce323fb5627c.mp4


