
// Setup
var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var mysql = require('mysql');
var fs = require('fs');
var router = express.Router();
var port = process.env.PORT || 12355;
var dbConnected = false;
var dbConnectionInfo = require('./config')
var connection = mysql.createConnection(dbConnectionInfo.config);

app.use(bodyParser.urlencoded({extended:true}))
app.use(bodyParser.json())
app.use('/api', router)
app.use(function (err, req, res, next) {
  console.log("ERROR");
  console.log(err.message);
  return res.status(404).json({message: err.message});
});


connection.connect(function(err) {
  if(err) throw err;
  console.log('Connected');
  dbConnected = true;
});


router.get('/', function(req, res){
  //res.json({message: 'api works'});
});

router.route('/player/create')
  // Create a new player
  .post(function(req, res, next) {
    var player_id = req.body.player_id;
    var player_name = req.body.player_name;
    if(player_id == undefined || player_name == undefined) {
      return res.json({message: "undefined values"})
    }
    else {
      var sql = mysql.format("INSERT INTO player (PLAYER_id, name, points, distance)\
                              VALUES(?, ?, 0, 0.0)", [player_id, player_name]);

      // Database stuff. First add player to PLAYER, then add info to BEST_TIME
      connection.query(sql, function(err, result, fields){
        if(err) {
          return next(err);
        }
        sql = mysql.format("INSERT INTO best_time (level_id, time, PLAYER_id, timestamp)\
                            VALUES (0, 99.9999, ?, now()),\
                             (1, 99.9999, ?, now()),\
                             (2, 99.9999, ?, now());", [player_id, player_id, player_id]);
        connection.query(sql, function(err, result, fields){
          if(err) {
            return next(err);
          }
          res.json({message: "success"});
        });
      });
    }
  });

router.route('/player/:player_id')
  // Get player info
  .get(function(req, res, next) {
    var sql = mysql.format("SELECT * FROM player WHERE PLAYER_id = ?", [req.params.player_id]);
    connection.query(sql, function(err, result, fields){
      if(err) return next(err);
      res.json(result);
    });
  })

  // Update player info
  .post(function(req, res, next) {
    var player_id = req.params.player_id;
    var player_name = req.body.player_name;
    var points = req.body.points;
    var distance = req.body.distance;

    if(player_name == undefined || points == undefined || distance == undefined) {
      console.log("UPDATE: undefined values");
      res.json({message: "undefined values"})
    }

    console.log("UPDATE: " + player_id);
    var sql = mysql.format("UPDATE player\
                            SET name=?, points=?, distance=?\
                            WHERE PLAYER_id=?", [player_name,points,distance,player_id]);
    connection.query(sql, function(err, result, fields){
      if(err) return next(err);
      res.json(result);
    });
  });

router.route('/player/ghost/:player_id/:level_id')

  // Get ghost data for player_id, level_id
  .get(function(req, res, next) {
    fs.readFile('ghostData/'+req.params.player_id+'_'+req.params.level_id+'.gst', function(err, data) {
      if(err) return next(err);
      res.send(data);
    });
  })

  // Create/Update ghost data for player_id, level_id
  .post(function(req, res) {
    var jsonString = JSON.stringify(req.body, null, 4);
    fs.writeFile('ghostData/'+req.params.player_id+'_'+req.params.level_id+'.gst', jsonString, function(err) {
      if(err) return next(err);
      res.json();
    });
  });

router.route('/best_time/:level_id')
.get(function(req, res, next) {
  var level_id = req.params.level_id;

  // Distance high score list
  if(level_id == -1)
  {
    var sql = "SELECT PLAYER_id, name, distance as time\
               FROM player\
               ORDER BY distance DESC"
     connection.query(sql, function(err, result, fields){
       if(err) return next(err);
       res.json(result);
     });
  }
  else
  {
    var sql = mysql.format("SELECT PLAYER_id, name, time FROM best_time JOIN player USING(PLAYER_id) WHERE level_id = ? ORDER BY time, timestamp", [req.params.level_id]);
    connection.query(sql, function(err, result, fields){
      if(err) return next(err);
      res.json(result);
    });
  }
});

router.route('/best_time/:player_id/:level_id')
  // Gets high scores of player_id on level_id
  /*.get(function(req, res, next) {
    var sql = mysql.format("SELECT name, time FROM best_time JOIN player USING(PLAYER_id) WHERE level_id = ? ORDER BY time", [req.params.level_id]);
    connection.query(sql, function(err, result, fields){
      if(err) return next(err);
      res.json(result);
    });
  })*/

  // Updates high score of player_id on level_id
  .post(function(req, res, next){
    var player_id = req.params.player_id;
    var level_id = req.params.level_id;
    var new_time = req.body.time;

    if(new_time == undefined) {
      res.json({message: "undefined values"})
    }

    var sql = mysql.format("UPDATE best_time\
                            SET time=?, timestamp=now()\
                            WHERE PLAYER_id=? AND level_id=?", [new_time,player_id,level_id]);
    connection.query(sql, function(err, result, fields){
      if(err) return next(err);
      if(result.affectedRows == 0) return next(new Error());
      res.json(result);
    });
  });

// Start server
app.listen(port);
