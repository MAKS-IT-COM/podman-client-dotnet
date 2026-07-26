#requires -Version 7.0
#requires -PSEdition Core

$script:ExternalCommandTestHandler = $null
$script:ExternalCommandAvailability = @{}

function Set-ExternalCommandTestHandler {
    param(
        [Parameter(Mandatory = $true)]
        [scriptblock]$Handler
    )

    $script:ExternalCommandTestHandler = $Handler
}

function Clear-ExternalCommandTestHandler {
    $script:ExternalCommandTestHandler = $null
}

function Set-ExternalCommandAvailability {
    param(
        [Parameter(Mandatory = $true)]
        [hashtable]$Availability
    )

    $script:ExternalCommandAvailability = @{}
    foreach ($key in $Availability.Keys) {
        $script:ExternalCommandAvailability[[string]$key] = [bool]$Availability[$key]
    }
}

function Invoke-ExternalCommand {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Name,

        [string[]]$ArgumentList = @(),

        [string]$WorkingDirectory,

        [string]$InputObject,

        [switch]$MergeErrorOutput
    )

    $previousLocation = $null
    if (-not [string]::IsNullOrWhiteSpace($WorkingDirectory)) {
        $previousLocation = Get-Location
        Push-Location $WorkingDirectory
    }

    try {
        $effectiveWorkingDirectory = (Get-Location).Path

        if ($null -ne $script:ExternalCommandTestHandler) {
            $handlerResult = & $script:ExternalCommandTestHandler `
                -Name $Name `
                -ArgumentList $ArgumentList `
                -WorkingDirectory $effectiveWorkingDirectory `
                -InputObject $InputObject `
                -MergeErrorOutput:$MergeErrorOutput.IsPresent

            $global:LASTEXITCODE = [int]$handlerResult.ExitCode
            if ($null -eq $handlerResult.Output) {
                return @()
            }

            if ($handlerResult.Output -is [System.Collections.IEnumerable] -and -not ($handlerResult.Output -is [string])) {
                return @($handlerResult.Output)
            }

            return @([string]$handlerResult.Output)
        }

        if ($script:ExternalCommandAvailability.ContainsKey($Name) -and -not $script:ExternalCommandAvailability[$Name]) {
            throw "External command '$Name' is marked unavailable."
        }

        if (-not [string]::IsNullOrWhiteSpace($InputObject)) {
            $output = $InputObject | & $Name @ArgumentList 2>&1
        }
        elseif ($MergeErrorOutput) {
            $output = & $Name @ArgumentList 2>&1
        }
        else {
            $output = & $Name @ArgumentList
        }

        return @($output)
    }
    finally {
        if ($null -ne $previousLocation) {
            Pop-Location
        }
    }
}

Export-ModuleMember -Function `
    Invoke-ExternalCommand, `
    Set-ExternalCommandTestHandler, `
    Clear-ExternalCommandTestHandler, `
    Set-ExternalCommandAvailability
