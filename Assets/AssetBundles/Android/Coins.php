<!DOCTYPE  html>

<?php 
$fm = date('m',strtotime('-1 Monday'));
$fd = date('d',strtotime('-1 Monday'));
$fyear = date('Y',strtotime('-1 Monday'));
$tm = date('m');
$td = date('d');
$tyear = date('Y');

/*
$fm = "05";
$fd = "01";
$tyear = "2023";
$tm = "03";
$td = "01";
*/

if(isset($_REQUEST["fm"]))
{
	$fm = $_REQUEST["fm"];
	$fd = $_REQUEST["fd"];
	$tm = $_REQUEST["tm"];
	$td = $_REQUEST["td"];
	$tyear = $_REQUEST["tyear"];
}
include ("admin/configurations/connect.php");
?>

<html lang="en">

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Ranking by Coins - Fulbo Galaxy</title>
    <meta name="description" content="Home page for Fulbo Galaxy, the DeFi Revolution" />
    <meta name="author" content="Aconcagua Games" />
    <link rel="icon" href="/logo.png" type="image/png" />	
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
                    <h1>Ranking by Coins Grabbed (only for Twitter Users)</h1>
                </div>
				 <div class="ranking-data">
                    <h2>From: <span><?php echo $fyear; ?>-<?php echo $fm; ?>-<?php echo $fd; ?></span></h2>
                    <h2>To: <span><?php echo $tyear; ?>-<?php echo $tm; ?>-<?php echo $td; ?></span></h2>
                </div>
                <!-- Tabla Posiciones -->
                <div class="position-table">
                    <table>
                        <tr>
                          <th>#</th>
                          <th>Twitter</th>
                          <th>Team</th>
                          <th>Coins Grabbed</th>
                        </tr>
						
						<?php
$query = "SELECT 
	users.twitter,
    users.email,
    users.user,
    SUM(tracking.coins_grabbed) as coins_grabbed
FROM 
	tracking
INNER JOIN	
	users ON users.email = tracking.email
WHERE tracking.timestamp BETWEEN '" . $fyear . "-" . $fm . "-" . $fd . " 00:00:01' AND '" . $tyear . "-" . $tm . "-" . $td . " 23:59:59'
GROUP BY 
	tracking.email
ORDER BY
	coins_grabbed DESC
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
		if($row["twitter"] != "")
		{	
		$twitterName = $row["twitter"];
		if($twitterName[0] != "@")
			$twitterName = "@" . $twitterName;
					?>
				
                        <tr>
                          <td><?php echo $num; ?></td>
                          <td><?php echo $twitterName ?></td>
                          <td><?php echo $row["user"]; ?></td>
                          <td><?php echo $row["coins_grabbed"]; ?></td>					  
                        </tr>
	<?php 
		$num++;
		} 
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


