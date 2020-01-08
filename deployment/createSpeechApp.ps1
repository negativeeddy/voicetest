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
$luisResourceName = "$siteName-luisauthoringkey"

# create the custom speech app
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

invoke-restmethod -Method POST -Uri "https://westus2.commands.speech.microsoft.com/apps" -Body (ConvertTo-Json $body) -Header $headers

write-host $response.appId