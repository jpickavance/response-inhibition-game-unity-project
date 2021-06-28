mergeInto(LibraryManager.library,
{
    fullscreenMenuListener: function()
    {
        if(document.addEventListener)
        {
            document.addEventListener('fullscreenchange', exitHandler3, false);
            document.addEventListener('mozfullscreenchange', exitHandler3, false);
            document.addEventListener('MSFullscreenChange', exitHandler3, false);
            document.addEventListener('webkitfullscreenchange', exitHandler3, false);
            
        }
        function exitHandler3()
        {
            if (document.webkitIsFullScreen || document.mozFullScreen || (document.msFullscreenElement !== null && document.msFullscreenElement !== undefined))
            {
                SendMessage('MainMenu', 'ToggleFullscreen', 'fullscreen');
            }
            else if (!document.webkitIsFullScreen || !document.mozFullScreen || (document.msFullscreenElement == null && document.msFullscreenElement !== undefined))
            {
                SendMessage('MainMenu', 'ToggleFullscreen', 'exitFullscreen');
            }
        }
    },
    fullscreenListener: function() //could collapse this into function above if I can get the unity scene from js
    {
        if(document.addEventListener)
        {
            document.addEventListener('fullscreenchange', exitHandler, false);
            document.addEventListener('mozfullscreenchange', exitHandler, false);
            document.addEventListener('MSFullscreenChange', exitHandler, false);
            document.addEventListener('webkitfullscreenchange', exitHandler, false);
            
        }
        function exitHandler()
        {
            if (document.webkitIsFullScreen || document.mozFullScreen || (document.msFullscreenElement !== null && document.msFullscreenElement !== undefined))
            {
                SendMessage('Pause', 'ToggleFullscreen', 'fullscreen');
            }
            else if (!document.webkitIsFullScreen || !document.mozFullScreen || (document.msFullscreenElement == null && document.msFullscreenElement !== undefined))
            {
                SendMessage('Pause', 'ToggleFullscreen', 'exitFullscreen');
            }
        }
    },
    lockListener: function()
    {
        if(document.addEventListener)
        {
            document.addEventListener('pointerlockchange', exitHandler2, false);
            document.addEventListener('mozpointerlockchange', exitHandler2, false);
            document.addEventListener('webkitpointerlockchange', exitHandler2, false);
            document.addEventListener('mspointerlockchange', exitHandler2, false);
        }

      function exitHandler2()
      {
        var canvas = document.getElementById('#canvas');
        if (document.pointerLockElement === canvas || document.mozPointerLockElement === canvas || document.webkitPointerLockElement === canvas || document.msPointerLockElement === canvas)
        {
          SendMessage('Pause', 'ToggleLock', 'lock');
        }
        else
        {
          SendMessage('Pause', 'ToggleLock', 'unlock');
        }
      }
    },
    UnloadListener: function()
    {
        window.addEventListener('beforeunload', function(e){
            e.preventDefault(); // If you prevent default behavior in Mozilla Firefox prompt will always be shown
            // Chrome requires returnValue to be set
            var dialogText = "Your progress has been saved! Would you like to exit the page?";
            e.returnValue = dialogText;
            SendMessage('OnCloseListener', 'OnClose');
        });
    },
    getScreenWidth: function()
    {
        var widthPx = String(screen.width);
        var bufferSize = lengthBytesUTF8(widthPx) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(widthPx, buffer, bufferSize);
        return buffer;
    },
    getScreenHeight: function()
    {
        var heightPx = String(screen.height);
        var bufferSize = lengthBytesUTF8(heightPx) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(heightPx, buffer, bufferSize);
        return buffer;
    },
    getPixelRatio: function()
    {
        var pxRatio = String(window.devicePixelRatio);
        var bufferSize = lengthBytesUTF8(pxRatio) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(pxRatio, buffer, bufferSize);
        return buffer;
    },
    getBrowserVersion: function()
    {
        function get_browser() 
        {
            var ua=navigator.userAgent,tem,M=ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || []; 
            if(/trident/i.test(M[1])){
                tem=/\brv[ :]+(\d+)/g.exec(ua) || []; 
                return {name:'IE',version:(tem[1]||'')};
                }   
            if(M[1]==='Chrome'){
                tem=ua.match(/\bOPR|Edge\/(\d+)/)
                if(tem!=null)   {return {name:'Opera', version:tem[1]};}
                }   
            M=M[2]? [M[1], M[2]]: [navigator.appName, navigator.appVersion, '-?'];
            if((tem=ua.match(/version\/(\d+)/i))!=null) {M.splice(1,1,tem[1]);}
            return {
            name: M[0],
            version: M[1]}
        };
        var browser = get_browser();
        var browserName = String(browser.name);
        var version = String(browser.version);
        var browserVersion = browserName + ", " + version;
        var bufferSize = lengthBytesUTF8(browserVersion) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(browserVersion, buffer, bufferSize);
        return buffer;
    },
    ReadData: function (tableName, token)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "tokenId": Pointer_stringify(token)
            },
            AttributesToGet: [
                "tokenId", "available", "gameProgress", "trial", "SSD", "score"
            ]
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.get(params, function(err, data)
        {
            if (err)
            {
                console.log(err);
                var returnStr = "Please enter a valid secret key";
                SendMessage('MainMenu', 'ErrorCallback', returnStr);
            }
            else if (data.Item == undefined)
            {
                var returnStr = "The token you have entered isn't recognised";
                SendMessage('MainMenu', 'ErrorCallback', returnStr);
            }
            else
            {
                console.log(data);
                var returnStr = JSON.stringify(data.Item);
                SendMessage('MainMenu', 'StringCallback', returnStr);
            }
        });
    },
    UpdateToken: function (tableName, token)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "tokenId": Pointer_stringify(token)
            },
            UpdateExpression: "set available = :a",
            ExpressionAttributeValues:
            {
                ":a": false
            },
            ReturnValues:"UPDATED_NEW"
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.update(params, function(err, data)
        {
            if (err)
            {
                console.log(err);
            }
            else if (data )
            {
                console.log("tokenId in tokenTable updated");
            }
        });
    },
    InsertData: function (tableName, token, trialNum, trialStartTime, certaintyCond, stopCond, SSD, 
                          moved, hit, mouseZero, setupTime, enterTime, holdTime, stopTime, initiationTime, movementTime, feedbackTime,
                          timeData, targetPosxData, posyData, yInputData, xInputData)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            {
                "tokenId": Pointer_stringify(token),
                "trial": Pointer_stringify(trialNum),
                "trialStartTime": Pointer_stringify(trialStartTime),
                "certaintyCond": Pointer_stringify(certaintyCond),
                "stopCond": Pointer_stringify(stopCond),
                "SSD": Pointer_stringify(SSD),
                "moved": Pointer_stringify(moved),
                "hit": Pointer_stringify(hit),
                "mouseZero": Pointer_stringify(mouseZero),
                "setupTime": Pointer_stringify(setupTime),
                "enterTime": Pointer_stringify(enterTime),
                "holdTime": Pointer_stringify(holdTime),
                "stopTime": Pointer_stringify(stopTime),
                "initiationTime": Pointer_stringify(initiationTime),
                "movementTime": Pointer_stringify(movementTime),
                "feedbackTime": Pointer_stringify(feedbackTime),
                "frameTime": Pointer_stringify(timeData),
                "targetPos": Pointer_stringify(targetPosxData),
                "pos_y": Pointer_stringify(posyData),
                "y_input": Pointer_stringify(yInputData),
                "x_input": Pointer_stringify(xInputData) 
            }
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        var returnStr = "Error";
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Inserted:" + JSON.stringify(data, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
        });
    },
    InsertUser: function (tableName, token, age, gender, widthPx, heightPx, pxRatio, browserVersion, handedness, pointer, mouseSensitivity, consentTime, tutorial1Trials, tutorial2Trials, startTime)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            {
                "tokenId": Pointer_stringify(token),
                "age": Pointer_stringify(age),
                "gender": Pointer_stringify(gender),
                "widthPx": Pointer_stringify(widthPx),
                "heightPx": Pointer_stringify(heightPx),
                "pxRatio": Pointer_stringify(pxRatio),
                "browserVersion": Pointer_stringify(browserVersion),
                "handedness": Pointer_stringify(handedness),
                "pointer" : Pointer_stringify(pointer),
                "mouseSensitivity": Pointer_stringify(mouseSensitivity),
                "consentTime": Pointer_stringify(consentTime),
                "tutorial1Trials": Pointer_stringify(tutorial1Trials),
                "tutorial2Trials": Pointer_stringify(tutorial2Trials),
                "startTime": Pointer_stringify(startTime)
            }
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        var returnStr = "Error";
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Inserted:" + JSON.stringify(data, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
        });
    },
    InsertLeaderboardUser: function (tableName, token, score, rSSRT, hitPerc, pSSRT, comboHigh, falseStarts)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            {
                "tokenId": Pointer_stringify(token),
                "score": score,
                "rSSRT": Pointer_stringify(rSSRT),
                "hitPerc": Pointer_stringify(hitPerc),
                "pSSRT": Pointer_stringify(pSSRT),
                "comboHigh": Pointer_stringify(comboHigh),
                "falseStarts": Pointer_stringify(falseStarts)
            }
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        var returnStr = "Error";
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                console.log(err);
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Inserted:" + JSON.stringify(data, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
        });
    },
    UpdateUser: function (tableName, token, pauses, trialsPaused)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "tokenId": Pointer_stringify(token)
            },
            UpdateExpression: "set n_pauses = :n, trials = :t",
            ExpressionAttributeValues:
            {
                ":n": Pointer_stringify(pauses),
                ":t": Pointer_stringify(trialsPaused)
            },
            ReturnValues:"UPDATED_NEW"
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.update(params, function(err, data)
        {
            if (err)
            {
                console.log(err);
            }
            else if (data )
            {
                console.log("summary data added to user table");
            }
        });
    },
    UpdateAimVars: function (tableName, token, order, zeroAimQuestions,
        CertainTargetPos, CertainTargetHitX, CertainTargetHitY, CertainBatHitX, CertainBatHitY, 
        UncertainTargetPos, UncertainTargetHitX, UncertainTargetHitY, UncertainBatHitX, UncertainBatHitY,
        ZeroOrder, ZeroCertainTargetPos, ZeroCertainTargetHitX, ZeroCertainTargetHitY, ZeroCertainBatHitX, ZeroCertainBatHitY,
        ZeroUncertainTargetPos, ZeroUncertainTargetHitX, ZeroUncertainTargetHitY, ZeroUncertainBatHitX, ZeroUncertainBatHitY)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "tokenId": Pointer_stringify(token)
            },
            UpdateExpression: "set CertaintyOrder = :o, ZeroAimQuestions = :z, CertainTargetPos = :a, CertainTargetHitX = :b, CertainTargetHitY = :c, CertainBatHitX = :d, CertainBatHitY = :e, UncertainTargetPos = :f, UncertainTargetHitX = :g, UncertainTargetHitY = :h, UncertainBatHitX = :i, UncertainBatHitY = :j, ZeroCertaintyOrder = :k, ZeroCertainTargetPos = :l, ZeroCertainTargetHitX = :m, ZeroCertainTargetHitY = :n, ZeroCertainBatHitX = :p, ZeroCertainBatHitY = :q, ZeroUncertainTargetPos = :r, ZeroUncertainTargetHitX = :s, ZeroUncertainTargetHitY = :t, ZeroUncertainBatHitX = :u, ZeroUncertainBatHitY = :v",
            ExpressionAttributeValues:
            {
                ":o": Pointer_stringify(order),
                ":z": zeroAimQuestions,
                ":a": CertainTargetPos,
                ":b": CertainTargetHitX,
                ":c": CertainTargetHitY,
                ":d": CertainBatHitX,
                ":e": CertainBatHitY,
                ":f": UncertainTargetPos,
                ":g": UncertainTargetHitX,
                ":h": UncertainTargetHitY,
                ":i": UncertainBatHitX,
                ":j": UncertainBatHitY,
                ":k": Pointer_stringify(ZeroOrder), 
                ":l": ZeroCertainTargetPos,
                ":m": ZeroCertainTargetHitX,
                ":n": ZeroCertainTargetHitY,
                ":p": ZeroCertainBatHitX,
                ":q": ZeroCertainBatHitY,
                ":r": ZeroUncertainTargetPos,
                ":s": ZeroUncertainTargetHitX,
                ":t": ZeroUncertainTargetHitY,
                ":u": ZeroUncertainBatHitX,
                ":v": ZeroUncertainBatHitY
            },
            ReturnValues:"UPDATED_NEW"
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.update(params, function(err, data)
        {
            if (err)
            {
                console.log(err);
            }
            else if (data )
            {
                console.log("summary data added to user table");
            }
        });
    },
    CompleteUser: function (tableName, token)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "tokenId": Pointer_stringify(token)
            },
            UpdateExpression: "set gameProgress = :g",
            ExpressionAttributeValues:
            {
                ":g": "complete"
            },
            ReturnValues:"UPDATED_NEW"
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.update(params, function(err, data)
        {
            if (err)
            {
                console.log(err);
            }
            else if (data)
            {
                console.log("tokenId in tokenTable updated");
            }
        });
    },
    SaveProgress: function (tableName, token, gameProgress, trial, SSD, score, windowClose)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "tokenId": Pointer_stringify(token)
            },
            UpdateExpression: "set gameProgress = :g, trial = :t, SSD = :s, score = :c, windowClose = :w",
            ExpressionAttributeValues:
            {
                ":g": Pointer_stringify(gameProgress),
                ":t": trial,
                ":s": SSD,
                ":c": score,
                ":w": windowClose
            },
            ReturnValues:"UPDATED_NEW"
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.update(params, function(err, data)
        {
            if (err)
            {
                console.log(err);
            }
            else if (data )
            {
                console.log("Game progress saved!");
            }
        });
    },
    GetLeaderboardSize: function (tableName)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName)
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var dynamodb = new AWS.DynamoDB();
        dynamodb.describeTable(params, function(err, data)
        {
            if (err)
            {
                var returnStr = "Unable to retrieve table length";
                SendMessage('Leaderboard', 'ErrorCallback', returnStr);
            }
            else
            {
                SendMessage('Leaderboard', 'setLeaderboardSize', data.Table.ItemCount);
            }
        });
    },
    ReadLeaderboardTop10: function (tableName)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            ProjectionExpression: "tokenId, score, comboHigh, hitPerc, rSSRT"
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "AKIAURBMNNXNQMTU6UGF",
            secretAccessKey: "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"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.scan(params, function(err, data)
        {
            if (err)
            {
                var returnStr = "Unable to retrieve items";
                SendMessage('Leaderboard', 'ErrorCallback', returnStr);
            }
            else
            {
                data.Items.forEach(function(element)
                    {
                        var itemString = JSON.stringify(element);
                        SendMessage('Leaderboard', 'appendResult', itemString);
                    });
            }
        });
    },
    OpenWindow: function(link)
    {
        var url = Pointer_stringify(link);
        document.onmouseup = function()
        {
            window.open(url);
            document.onmouseup = null;
        }
    },
});