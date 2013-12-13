properties {
    $binariesDirs = "VideoStore.ContentManagement, VideoStore.CustomerRelations, VideoStore.Operations, VideoStore.Sales"
	$buildConfiguration = "Debug"
	$UseEmulator = $true
	$StorageAccount = "youraccount"
	$StorageKey = "yourkey"
}

$baseDir = Split-Path (Resolve-Path $MyInvocation.MyCommand.Path)
$artifactsDir = "$baseDir\artifacts"
$toolsDir = "$baseDir\tools"
$zipExec = "$toolsDir\zip\7za.exe"

include $toolsDir\psake\buildutils.ps1

task Init {
	
	if(-not (Test-Path $artifactsDir)){	
		Create-Directory $artifactsDir
	}
			
}

task Clean { 
	if(Test-Path $artifactsDir){
		Delete-Directory $artifactsDir
	}
}

task Package -depends Clean, Init {

	$folders = $binariesDirs -split ", "
	
	$folders | % {
		$packageName =  $_			
		$archive = "$artifactsDir\$packageName.zip"
		exec { &$zipExec a -tzip $archive $baseDir\$packageName\bin\$buildConfiguration\** }			
	}
}

task Upload -depends Package{

	if($UseEmulator)
	{
		$context = New-AzureStorageContext -Local 
	}
	else
	{	
		$context = New-AzureStorageContext -StorageAccountName "$StorageAccount" -StorageAccountKey "$StorageKey"
	}
		
	ls -path $artifactsDir -file -recurse | Set-AzureStorageBlobContent -Container "endpoints" -Context $context -Force
}