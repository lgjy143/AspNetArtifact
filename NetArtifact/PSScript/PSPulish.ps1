
$MsbuildPath = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\amd64\msbuild.exe'
##$Msbuild15Path ='D:\soft\vs2017\MSBuild\15.0\Bin\msbuild.exe'

function GetMsBuildPath([switch] $Use32BitMsBuild)
{
	if($MsbuildPath -eq $null -or $MsbuildPath.Trim() -eq "") {
		$item = Get-ChildItem -Path "C:\Program Files (x86)"  -Include MSBuild.exe -Recurse | Where-Object {$_.DirectoryName.EndsWith("Bin\amd64")}
		$item = $item.VersionInfo | Sort-Object -Property FileVersion -Descending | Select-Object FileName -First 1
		$MsbuildPath = $item.FileName
		if($MsbuildPath) { 
			Write-Host "Searching for MSBuild.exe takes a long time. It is recommended to write the path '$MsbuildPath' to the variable `$MsbuildPath." 
		}
		else {
			throw "Search MSBuild.exe failed"
			exit 1
		}
	}
	
	if($Use32BitMsBuild) { $MsbuildPath = $MsbuildPath -replace "\\amd64","" }
	
	if(!(Test-Path $MsbuildPath -PathType Leaf)) { throw "MsBuild.exe was not found on this system."; exit 1 }

	return $MsbuildPath
}

function GetVisualStudioToolsVersion
{
    ## vs版本列表
    $vsCommandPromptPaths = @(
		@{Path=$MsbuildPath;Version="15.0";}
        @{Path=$env:VS140COMNTOOLS + 'VsDevCmd.bat';Version="14.0";}
        @{Path=$env:VS120COMNTOOLS + 'VsDevCmd.bat';Version="12.0";}
        @{Path=$env:VS110COMNTOOLS + 'VsDevCmd.bat';Version="11.0";}
        @{Path=$env:VS100COMNTOOLS + 'vcvarsall.bat';Version="10.0";}
    )

	$vsToolsVersion = $null
	foreach ($path in $vsCommandPromptPaths)
	{
		try
		{
			if (Test-Path -Path $path.Path)
			{
				##$vsToolsVersion ="/ToolsVersion:"+ $path.Version
				$vsToolsVersion ="/p:VisualStudioVersion="+ $path.Version
				
				break
			}
		}
		catch 
        { 
            throw $_.Exception
            exit 1
        }
	}
    ## 返回 MsBuild ToolsVersion 参数
	return $vsToolsVersion
}

function InvokeMsBuild(
    [Parameter(Position=0,Mandatory = $true)]
    [Alias("Path")]
    [string]$ProjectPath,

    [parameter(Mandatory=$false)]
    [string]$OutDir,

    [parameter(Mandatory=$false)]
    [string]$WebProjectOutputDir,

    [parameter(Mandatory=$false)]
    [switch] $Use32BitMsBuild,

    [parameter(Mandatory=$false)]
    [switch] $UseDebug
    )
{   
    $build= GetMsBuildPath -Use32BitMsBuild:$Use32BitMsBuild
    Write-Host "MsBuild Path: $build`n"

	##return "MsBuild Path: $build`n"

    $toolsVersion=GetVisualStudioToolsVersion

    $configuration="Release"
    if($UseDebug){$configuration="Debug"}

    if(-not [String]::IsNullOrEmpty($OutDir)){ $OutDir="/p:OutDir="+ $OutDir }

    if(-not [String]::IsNullOrEmpty($WebProjectOutputDir)){ $WebProjectOutputDir="/p:WebProjectOutputDir="+ $WebProjectOutputDir }

    Write-Host "Start Build ...`n"
    Write-Host $build $projectPath $toolsVersion /p:RunCodeAnalysis=false /consoleloggerparameters:ErrorsOnly /p:Configuration=$configuration /nologo /verbosity:quiet /maxcpucount $OutDir $WebProjectOutputDir `n
	
	##return "$build $projectPath $toolsVersion /p:RunCodeAnalysis=false /consoleloggerparameters:ErrorsOnly /p:Configuration=$configuration /nologo /verbosity:quiet /maxcpucount $OutDir $WebProjectOutputDir `n"

	$result = ."$build" $projectPath  $toolsVersion  /p:RunCodeAnalysis=false /consoleloggerparameters:ErrorsOnly /p:Configuration=$configuration  /p:DebugType=none /nologo /verbosity:quiet /maxcpucount $OutDir $WebProjectOutputDir
	if($result.length -gt 0)
	{ 
		Write-Output $result
		exit 1
	}
    Write-Host "Build To Completed`n"
    return 
}

##MSBuild "%~dp0EAS_ClothPieceWage.sln"  /t:reBuild /p:Configuration=Release;DeployOnBuild=true;VisualStudioVersion=12.0;PublishProfile=clothPieceWage

