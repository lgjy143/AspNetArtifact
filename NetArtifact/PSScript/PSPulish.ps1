
$Msbuild15Path = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\amd64\msbuild.exe'
##$Msbuild15Path ='D:\soft\vs2017\MSBuild\15.0\Bin\msbuild.exe'

function GetMsBuildPath([switch] $Use32BitMsBuild)
{
	## 判断是否存在MSBuild v15
	if(Test-Path -Path $Msbuild15Path)
	{
		return $Msbuild15Path
	}
	
    ## 获取MsBuild.exe的最新版本的路径。如果找不到MsBuild.exe，则抛出异常。
	$registryPathToMsBuildToolsVersions = 'HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\'
	if ($Use32BitMsBuild)
	{
		## 如果32位路径存在则使用它，否则将使用与当前系统位一致的路径。
		$registryPathTo32BitMsBuildToolsVersions = 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\MSBuild\ToolsVersions\'
		if (Test-Path -Path $registryPathTo32BitMsBuildToolsVersions)
		{
			$registryPathToMsBuildToolsVersions = $registryPathTo32BitMsBuildToolsVersions
		}
	}

	## 获取MsBuild最新版本所在目录的路径。
	$msBuildToolsVersionsStrings = Get-ChildItem -Path $registryPathToMsBuildToolsVersions | Where-Object { $_ -match '[0-9]+\.[0-9]' } | Select-Object -ExpandProperty PsChildName
	$msBuildToolsVersions = @{}
	$msBuildToolsVersionsStrings | ForEach-Object {$msBuildToolsVersions.Add($_ -as [double], $_)}
	$largestMsBuildToolsVersion = ($msBuildToolsVersions.GetEnumerator() | Sort-Object -Descending -Property Name | Select-Object -First 1).Value
	$registryPathToMsBuildToolsLatestVersion = Join-Path -Path $registryPathToMsBuildToolsVersions -ChildPath ("{0:n1}" -f $largestMsBuildToolsVersion)
	$msBuildToolsVersionsKeyToUse = Get-Item -Path $registryPathToMsBuildToolsLatestVersion
	$msBuildDirectoryPath = $msBuildToolsVersionsKeyToUse | Get-ItemProperty -Name 'MSBuildToolsPath' | Select -ExpandProperty 'MSBuildToolsPath'

	if(!$msBuildDirectoryPath)
	{
		throw 'The registry on this system does not appear to contain the path to the MsBuild.exe directory.'
	}

	## 获取MsBuild可执行文件的路径。
	$msBuildPath = (Join-Path -Path $msBuildDirectoryPath -ChildPath 'msbuild.exe')

	if(!(Test-Path $msBuildPath -PathType Leaf))
	{
		throw "MsBuild.exe was not found on this system at the path specified in the registry, '$msBuildPath'."
	}

	return $msBuildPath
}

function GetVisualStudioToolsVersion
{
    ## vs版本列表
    $vsCommandPromptPaths = @(
		@{Path=$Msbuild15Path;Version="15.0";}
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

