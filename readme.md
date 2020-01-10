# Deploying Azure Resources 
This project provides you with the scripts needed to deploy the necessary resources for a Microsoft Speech Studio demo. 

## Things you need first 
1. An [Azure](https://azure.microsoft.com/) subscription. 
2. PowerShell

## Deploying Azure Resources
1. Click the botton blow:<br/>[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)<br> 

2. A new window should pop up that prompts you to log into your Azure account. 

3. Once logged in your should be greeted with the below screen:
![Landing Page](doc/LandingPage.png)

4. Make sure the correct "Directory" and "Subscription" are selected.

5. Next, you are able to edit the name of "Resource Group Name", "Site Name", and input a value for "Signalr_service_name". Also, choose the "SiteLocation" of "West US 2". 
<br/>\*Recommendation: Make the names easy to remember. You will be using them later.<br>

6. Click "Next" at the bottom of the page.

7. The next page will list the Azure resources that will be deployed. Click "Deploy" to continue. 
![List of Resources](doc/ResourceList.png)

8. It will take a few minutes for all the resources to be deployed, but once it is complete you should see the following page. 
![Completed Deployment](doc/ResourceFinish.png)

9. You are now ready to deploy the Speech Services. 

## Deploying Speech Service
1. Clone the [VoiceTest](https://github.com/negativeeddy/voicetest) repo to your local machine.

2. In PowerShell, Navigate to the "deployment" folder of the cloned repo and run the createSpeechApp.ps1 script.
 ````
     \voicetest-master\voicetest-master> cd deployment
 ````
 ````
     .\createSpeechApp.ps1
 ````
3. The script will prompt you for the following information(theses resources will be in a resource group named the same as your sitename):

   •	siteName – whatever you used for the sitename in "Deploying Azure Resources"
   
   •	speechResourceKey – the key from the <siteName>-speech resource
   
   •	luisauthoringkey – the key from the <siteName>-luisauthoringkey resource
   
   •	your Azure subscription id

4. Once deployed your should get the following message:
````
Speech commands have been published.
````
You should see a SpeechApplicationId and a SpeechSubscriptionKey below the above message. Copy these down! You will need them for testing in the UWP app in the next section.

# Testing with UWP App
Once you have all the above resources created and deployed you are able to test using a UWP app. 

## Things you need first 
1. [Visual Studio](https://visualstudio.microsoft.com/vs/)
2. Clone the [SpeechCommandAccelerator](https://github.com/negativeeddy/SpeechCommandAccelerator) repo from GitHub
3. The SpeechApplicationId
4. The SpeechSubscriptionKey

## Setting up the UWP App

1. In Visual Studio, open the Microsoft.CognitiveServices.Inventory.sln solution file. 

2. In the "Solution Explorer", right click Microsoft.CognitiveServices.Inventory.UWP and select "Set as StartUp Project".


3. Expand the Microsoft.CognitiveServices.Inventory project and open "appSettings.json". Update this file with your SpeechApplicationId and SpeechSubscriptionKey from above.
![AppSettings](doc/AppSettings.png)

4. Save 

5. **Ensure Windows is in Develeper Mode** To build and deploy UWP apps locally, Windows needs to be in developer mode. You can ensure you have Developer Mode enabled by doing the following:

   1. In the taskbar, search for and open "Developer Settings"

   2. You will need to activate "Developer Mode" at the top of the menu. It should look like this when done: 
![Developer Settings](doc/DeveloperSettings.png)

6. Back in Visual Studio, run the Microsoft.CognitiveServices.Inventory.UWP by clicking the play button at the top of the screen:
![Run App](doc/RunApp.png)

7. A prompt will appear asking for permission to use your microphone. Give it permission.

8. The UWP app should now be running, and if everything it connected proplly you should see "Status: connected" at the top: 
![Connected](doc/Connected.png)

9.Open a browser window and go to the website you deployed in "Deploying Azure Resources" section (https://<sitename>.azurewebsites.net/). This will show your starting inventory. 

10. Click the Speak button in the UWP app and say "Delete 2 lattes". 

11. You will get a confirmation and see a 2  latte change in the deployed website via the signalR connection.


## Further Testing
Once the speech service and the azure resources have been deployed, its time to test!

1. Go to the [Speech Studio Portal](https://speech.microsoft.com/portal?noredirect=true) and login using the same account you used for the above deployments.

2. Once logged in, select <sitename>-speech(<sitename> is the name you provided in "Deploying Azure Resources" section) and click "Got to Studio" at the top of the page.
 
3. Click on <sitename>-commands

4. You should a page that looks similar to the one below:
![Speech Command](doc/SpeechCommand.png)

5. Open another browser tab and go to the website you deployed in "Deploying Azure Resources" section (https://<sitename>.azurewebsites.net/)

6. Go back to the Speech Portal and click "Train" located at the top right of the page. Wait for the message that confirms it has succeeded.

7. Click "Test" located near the "Train" button. 

8. In the test bot type 'delete 4 Lattes'
   • It is currently case sensitive for the drink types. 

9. You should see the 4 latte change in the deployed website via the signalR connection.
