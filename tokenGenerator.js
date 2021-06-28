function generateTokens(length)
{
    //configure dynamo db
    var AWS = require("aws-sdk");

    AWS.config.update({
        region: "eu-west-2",
        endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
        accessKeyId: "AKIAUZ3DVLNPGX6EDYNA",
        secretAccessKey: "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"
    });

    var docClient = new AWS.DynamoDB.DocumentClient();

    //generate and post n = length tokens
    for (i = 0; i < length; i++)
    {
        var token = [...Array(7)].map(i=>(~~(Math.random()*36)).toString(36)).join('');
        var params =
        {
            TableName: "JP_FBS_Pilot_TokenTable",
            Item:
            {
                "tokenId": token,
                "available": true,
                "gameProgress": "tutorial1",
                "trial": 0,
                "SSD": 780,
                "score": 0,
                "windowClose": false
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

//generateTokens(100);

function addTokensFromCsv()
{
    /*var csv = "201231055\n\
201320863\n\
201359115\n\
201260204\n\
201303156\n\
201388142\n\
201319827\n\
201216896\n\
201298528\n\
201255374\n\
201341420\n\
201255455\n\
201314844\n\
201328055\n\
201308523\n\
201349776\n\
201297205\n\
201057562\n\
201225992\n\
201317772\n\
201227917\n\
201326874\n\
201227757\n\
201296152\n\
201106975\n\
201373577\n\
201329023\n\
201462689\n\
201465717\n\
201381200\n\
201315339\n\
201258220\n\
201312935\n\
201308968\n\
201340664\n\
201339692\n\
201325993\n\
201306646\n\
201328923\n\
201315614\n\
201321938\n\
201262147\n\
201092164\n\
201303415\n\
201311320\n\
201094867\n\
201247077\n\
201308522\n\
201323913\n\
201332676\n\
201303163\n\
201314994\n\
201327921\n\
201335327\n\
201224952\n\
200856603\n\
201348778\n\
201385541\n\
201249091\n\
201226935\n\
201322591\n\
201320133\n\
201346906\n\
201319430\n\
201341587\n\
201220807\n\
201322304\n\
201331269\n\
201324727\n\
201310181\n\
201317810\n\
201344496\n\
201220079\n\
201300160\n\
201336716\n\
201349053\n\
201339752\n\
201216422\n\
201337266\n\
201385204\n\
201225142\n\
201319805\n\
201296111\n\
201326868\n\
201323918\n\
201304704\n\
201308562\n\
201317544\n\
201344985\n\
201349482\n\
201237272\n\
201345216\n\
201311597\n\
201388105\n\
201314139\n\
201316539\n\
201315330\n\
201299719\n\
201239945\n\
201211406\n\
201343888\n\
201338618\n\
201315966\n\
201299256\n\
201245228\n\
201318710\n\
201229432\n\
201221308\n\
201354277\n\
201323417\n\
201342650\n\
201333212\n\
201228785\n\
201320355\n\
201298566\n\
201373047\n\
201303702\n\
201342254\n\
201317695\n\
201314098\n\
201312539\n\
201160919\n\
201317898\n\
201330083\n\
201328988\n\
201360243\n\
201318403\n\
201317481\n\
201354924\n\
201256018\n\
201325484\n\
201095775\n\
201237886\n\
201334952\n\
201312359\n\
201333893\n\
201300411\n\
201315014\n\
201206712\n\
201330003\n\
201294758\n\
201314117\n\
201324752\n\
201306713\n\
201322137\n\
201335030\n\
201462053\n\
201321213\n\
201327703\n\
201203933\n\
201316302\n\
201310115\n\
201293406\n\
201303073\n\
201338654\n\
201107503\n\
201309606\n\
201318378\n\
201305841\n\
201315690\n\
201245141\n\
201342191\n\
201260764\n\
201148912\n\
201224641\n\
201371858\n\
201293134\n\
201347463\n\
201199520\n\
201245160\n\
201220846\n\
201013822\n\
201303839\n\
201342088\n\
201335140\n\
201352319\n\
201142529\n\
201329202\n\
201337915\n\
201306716\n\
201199307\n\
201327129\n\
201213297\n\
201309132\n\
201310853\n\
201307178\n\
201244184\n\
201211653\n\
201324650\n\
201305810\n\
201359386\n\
201390201\n\
201353379\n\
201334713\n\
201315923\n\
201302358\n\
201230144\n\
201317267\n\
201327722\n\
201324476\n\
201316227\n\
201303132\n\
201328364\n\
201323376\n\
201238745\n\
201319866\n\
201211903\n\
201463983\n\
201316028\n\
201303306\n\
201229683\n\
201152376\n\
201311354\n\
201319898\n\
201358124\n\
201330484\n\
201307116\n\
201002760\n\
201365550\n\
201307069\n\
201314099\n\
201315839\n\
201308092\n\
201387962\n\
201377273\n\
201299210\n\
201371860\n\
test1\n\
test2\n\
test3\n\
test4\n\
test5\n\
test6\n\
test7\n\
test8\n\
test9\n\
test10\n\
test99";
*/

var csv = "RyanMorehead";


    var token_arr = csv.split("\n");                
    //configure dynamo db
    var AWS = require("aws-sdk");

    AWS.config.update({
        region: "eu-west-2",
        endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
        accessKeyId: "AKIAURBMNNXNQMTU6UGF",
        secretAccessKey: "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"
    });

    var docClient = new AWS.DynamoDB.DocumentClient();

    //generate and post n = length tokens
    for (i = 0; i < token_arr.length; i++)
    {
        var token = token_arr[i];
        var params =
        {
            TableName: "JP_FBS_Pilot_TokenTable",
            Item:
            {
                "tokenId": token,
                "available": true,
                "gameProgress": "tutorial1",
                "trial": 0,
                "SSD": 780,
                "score": 0,
                "windowClose": false
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

/*function PopulateLeaderboard()
{
    var AWS = require("aws-sdk");

        AWS.config.update({
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        });

        var docClient = new AWS.DynamoDB.DocumentClient();

        //generate and post n = length tokens
        for (i = 0; i < 10; i++)
        {
            var params =
            {
                TableName: "JP_FBS_Pilot_Leaderboard",
                Item:
                {
                    "tokenId": "aaa" + i,
                    "score": 0,
                    "rSSRT": "-",
                    "hitPerc": "-",
                    "pSSRT": "-",
                    "comboHigh": "-",
                    "falseStarts": "-"
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
                    console.log("leaderboard successfully populated")
                }
            });
        }
    }
    PopulateLeaderboard()
    */
