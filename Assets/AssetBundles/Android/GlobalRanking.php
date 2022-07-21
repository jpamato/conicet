<!DOCTYPE  html>

<?php 

include ("admin/configurations/connect.php");
include ("Utils.php");
?>
<script>
function OnStadiumSelect(level){
	var myselect = document.getElementById("StadiumsSelect");
	var url = "/fulbogalaxy/Ranking.php?level=" + level;	
	url += "&stadium=" + myselect.options[myselect.selectedIndex].value;	
	window.location.href = url;
	
}
</script>
<html>
	<head>
		<title>Rankings</title>
        <meta charset="UTF-8">
        <link href="styles/ranking_style.css" rel="stylesheet" type="text/css">
		<meta name="viewport" content="width=device-width, initial-scale=1">
        <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet">
	</head>
	<body>
        <div class="main">
            <div class="container">
                <!-- Ranking -->
                <div class="ranking-title">
					<div class="back"><i class="fa fa-arrow-left"></i><a href = "Rankings.php">All Rankings</a></div>
                    <h1>Global Ranking by Score</h1>
                </div>
                <!-- Tabla Posiciones -->
                <div class="position-table">
                    <table>
                        <tr>
                          <th>#</th>
                          <th>Team</th>
                          <th>Twitter</th>
                          <th>Score</th>
                          <th>Result</th>
                          <th>Total Matches</th>
                        </tr>
						
						<?php
$query = "SELECT 
	users.email,
	users.user,
	users.twitter,
	users.gamesPlayed,
	tracking.score,
	tracking.score_team1,
	tracking.score_team2
FROM 
	tracking
INNER JOIN	
	users ON users.email = tracking.email
WHERE 
	tracking.score = 
(
	SELECT MAX(t2.score) FROM tracking t2 WHERE t2.email = tracking.email
) 
GROUP BY 
	tracking.email
ORDER BY
	tracking.score DESC
LIMIT 
	100;";

//WHERE
//`stadium` = '$stadium' AND
//`level` = '$level' 
//ORDER BY `score` DESC  LIMIT 100";


$statement=$pdo->prepare($query); 
$statement->execute(); 
$num = 1;
if (!$statement){ 
	echo 'Error al ejecutar la consulta'; 
}else{ 
	$array = $statement->fetchAll( PDO::FETCH_ASSOC );
	foreach($array as $row) {
?>
				
                        <tr>
                          <td><?php echo $num; ?></td>
                          <td><?php echo $row["user"]; ?></td>
                          <td><?php echo $row["twitter"]; ?></td>
                          <td><?php echo $row["score"]; ?></td>		
                          <td><?php echo $row["score_team2"] . "-" . $row["score_team1"]; ?></td>		
                         <td><?php echo $row["gamesPlayed"]; ?></td>				  
                        </tr>
<?php 
	$num++;
	} 
}
?>
                      </table>
                </div>
            </div>
        </div>
        <!-- Imagenes -->
        <div class="imagen_animacion izquierda">
            <img src="images/rocketkim.png" alt="Rocket" style="width: 80%;">
        </div>
        <div class="imagen_animacion derecha">
            <img src="images/rocketgirl.png" alt="Rocket" style="transform: rotate(-10deg);">
        </div>
	</body>
</html>


