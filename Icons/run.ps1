$phantomjs = "C:\Users\daniel.lidstrom\Downloads\phantomjs-2.0.0-windows\bin\phantomjs.exe"

$iconSizes = @(
	152
	144
	76
	72
	120
	114
	57
)

foreach ($size in $iconSizes) {
	"-" * 64
	$zoomFactor = $size / 32
	$filename = "apple-touch-icon-$($size)x$($size).png"
	$format = "$($size)px*$($size)px"
	&  $phantomjs "rasterize.js" "minaglosor.html" $filename $format $zoomFactor
}


$imageSizes = @(
	,@(1536, 2008)
	,@(1496, 2048)
	,@(768, 1004)
	,@(748, 1024)
	,@(1242, 2148)
	,@(1182, 2208)
	,@(750, 1294)
	,@(640, 1096)
	,@(640, 920)
	,@(320, 460)
)

foreach ($size in $imageSizes) {
	"-" * 64
	$zoomFactor = $size[0] / 32
	$filename = "apple-touch-startup-image-$($size[0])x$($size[1]).png"
	$format = "$($size[0])px*$($size[1])px"
	&  $phantomjs "rasterize.js" "minaglosor.html" $filename $format $zoomFactor
}
