#Requires -Version 6

Param(
    [string] $speechResourceKey,
    [string] $siteName,
    [string] $azureSubscriptionId,
    [string] $resourceGroup,
    [string] $luisAuthoringKey,
    [string] $luisAuthoringRegion = "westus"
)

# Get mandatory parameters
if (-not $siteName) {
    $name = Read-Host "? the site name you used when deploying to Azure"
}

if (-not $resourceGroup) {
	$resourceGroup = $siteName
}

$speechAppName = "$siteName-commands"
$speechResourceName = "$siteName-speech"
$luisResourceName = "$siteName-luisauthoringkey"

# create the custom speech app
write-host "Creating the speech custom command project $speechAppName"
$body = @{
  name = $speechAppName
  stage = "default"
  culture = "en-us"
  description = "updating the speech solution accelerator"
  skillEnabled = "true"
  luisAuthoringResourceId = "/subscriptions/$azureSubscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.CognitiveServices/accounts/$luisResourceName"
  luisAuthoringKey = $luisAuthoringKey
  luisAuthoringRegion = $luisAuthoringRegion
}

$headers = @{
    "Ocp-Apim-Subscription-Key" = $speechResourceKey
    "accept" = "application/json"
    "Content-Type" = "application/json-patch+json"
    }

try {
    $response = invoke-restmethod -Method POST -Uri "https://westus2.commands.speech.microsoft.com/apps" -Body (ConvertTo-Json $body) -Header $headers
} catch {
    # Dig into the exception to get the Response details.
    # Note that value__ is not a typo.
    Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__ 
    Write-Host "StatusDescription:" $_.Exception.Response.StatusDescription
    exit
}

$appId = $response.appId
write-host "Created project Id $appId"
write-host $response

# update the dialog model of the app
write-host "populating $speechAppName with the inventory commands model"
try {
    $response = invoke-restmethod -Method PUT -Uri "https://westus2.commands.speech.microsoft.com/apps/$appId/stages/default/cultures/en-us" -InFile "inventoryManagement.json" -Header $headers
} catch {
    # Dig into the exception to get the Response details.
    # Note that value__ is not a typo.
    Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__ 
    Write-Host "StatusDescription:" $_.Exception.Response.StatusDescription
    exit
}
write-host "done."

Read-Host "please open a browser to https://speech.microsoft.com/portal/$speechResourceName/CustomCommands/$appId and update the LUIS prediction resource in the settings section. Then press enter here to continue."

#start the training for the model
write-host "starting the training"
$response = invoke-restmethod -Method POST -Uri "https://westus2.commands.speech.microsoft.com/apps/$appId/stages/default/cultures/en-us/train" -Header $headers
$versionId = $response.versionId
write-host -NoNewline "training version $versionId"

#wait until the training is complete
try {
    $response = invoke-restmethod -Method GET -Uri "https://westus2.commands.speech.microsoft.com/apps/$appId/stages/default/cultures/en-us/train/$versionId" -Header $headers
} catch {
    # Dig into the exception to get the Response details.
    # Note that value__ is not a typo.
    Write-Host $response
    exit
}


while($response.completed -ne "true")
{
    start-sleep -seconds 1
    write-host -NoNewline "."
    $response = invoke-restmethod -Method GET -Uri "https://westus2.commands.speech.microsoft.com/apps/$appId/stages/default/cultures/en-us/train/$versionId" -Header $headers
}
write-host "training is completed"

#publish the model
write-host "publishing the model"
$response = invoke-restmethod -Method POST -Uri "https://westus2.commands.speech.microsoft.com/apps/$appId/stages/default/cultures/en-us/publish/$versionId" -Header $headers
write-host "model is published"

#print out the relevant info for the user to put in the application