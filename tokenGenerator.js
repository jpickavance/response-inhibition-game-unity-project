function generateTokens(length)
{
    //configure dynamo db
    var AWS = require("aws-sdk");

    AWS.config.update({
        region: "eu-west-2",
        endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
        accessKeyId: "YOUR ACCESS ID",
        secretAccessKey: "YOUR SECRET KEY"
    });

    var docClient = new AWS.DynamoDB.DocumentClient();

    //generate and post n = length tokens
    for (i = 0; i < length; i++)
    {
        var token = [...Array(7)].map(i=>(~~(Math.random()*36)).toString(36)).join('');
        var params =
        {
            TableName: "tokenTable",
            Item:
            {
                "tokenId": token,
                "available": true,
            }
        }
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                console.log(err)
            }
            else
            {
                console.log("token table successfully updated")
            }
        });
    }
}

//enerateTokens(100);

function addTokensFromCsv()
{
    var csv = "1\n\
2\n\
n";

    var token_arr = csv.split("\n");                
    //configure dynamo db
    var AWS = require("aws-sdk");

    AWS.config.update({
        region: "eu-west-2",
        endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
        accessKeyId: "YOUR ACCESS ID",
        secretAccessKey: "YOUR SECRET KEY"
    });

    var docClient = new AWS.DynamoDB.DocumentClient();

    //generate and post n = length tokens
    for (i = 0; i < token_arr.length; i++)
    {
        var token = token_arr[i];
        var params =
        {
            TableName: "tokenTable",
            Item:
            {
                "tokenId": token,
                "available": true,
            }
        }
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                console.log(err)
            }
            else
            {
                console.log("token table successfully updated")
            }
        });
    }
}
addTokensFromCsv()
