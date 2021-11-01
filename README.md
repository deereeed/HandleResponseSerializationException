Correct Error:

curl -X GET "http://localhost:5000/My/error-in-response-serialization?fooContentLength=23" -H  "accept: text/plain"


Fail:

curl -X GET "http://localhost:5000/My/error-in-response-serialization?fooContentLength=1000" -H  "accept: text/plain"
