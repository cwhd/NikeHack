# Custom Vision Hack

In this hack, we're going to show how to spin up your own front and back end apps in just a few steps.  After completing this hack, you'll be have a fully working app that uses custom vision AI to identify objects you select.

#### This hackathon excercies the following platforms or services

1. Visual Studio 2017
1. Xamarin (Xamarin.Forms)
1. Azure Functions (C#/.NET)
1. Azure Storage Account (blob)
1. Azure Cosmos DB (SQL/DocumentDB)
1. Cognitive Services Custom Vision

#### Recommended tools

1. (Required) [Visual Studio 2017](https://www.visualstudio.com/downloads/) with the following packages installed:
  1. Mobile Development with .NET
  1. Azure Development
1. [Postman](https://www.getpostman.com/)


### Step 1: Clone the Repo

Make a local directory, and then clone the repo from [https://github.com/rob-derosa/customvisionhack](https://github.com/rob-derosa/customvisionhack)

```git clone https://github.com/rob-derosa/customvisionhack.git```

### Step 2: Create a new Azure Function App

1. Browse to [https://portal.azure.com](https://portal.azure.com)
1. In the top left, click Create Resource > Compute > Function App
<br/><img src="resources/portal_create_new_functions_app.png" width="75%" />
1. Enter in a name for the app (e.g. `myfunctionsapp` 1. this must be unique 1. don't worry, the portal will tell you if it's not)
1. Choose your Azure Subscription
1. Choose "Create new" for the resource group and copy the same value you used for your app name into the Resource Group Name field (we'll create all of our resources under this same Resource Group)
1. Choose "Use existing", and select the resource group you created in the previous section
1. Choose a region (any region is fine)
1. Leave the rest of the settings as default
1. Optionally, for easy access, click the "Pin to dashboard" checkbox
1. Click "Create" to create your Function App
<br/><img src="resources/portal_create_new_functions_app_settings.png" width="75%" />
1. It can take a few minutes before this process completes 1. check the notifications 
<br/><img src="resources/portal_notifications_in_progress.png" width="40%" />
1. You can always check out all incoming notifications by clicking on the Alert icon in the top toolbar 
<br/><img src="resources/portal_notifications.png" width="40%" />


### Step 3: Create a Azure Cosmos DB

1. Once again, browse to [https://portal.azure.com](https://portal.azure.com)
1. In the top left, click Create Resource > Databases > Azure Cosmos DB
<br/><img src="resources/portal_create_new_cosmos_database.png" width="75%" />
1. Enter in a name for the database (e.g. `mycosmosdatabase`)
1. Choose the `SQL` from the API dropdown 1. this will use DocumentDB under the hood
1. Choose your Azure Subscription
1. Choose "Use existing", and select the resource group you created in the previous section
1. Choose a region (any region is fine)
1. Click "Create" to create the Cosmos DB
<br/><img src="resources/portal_create_new_cosmos_database_settings.png" width="75%" />
1. It can take a few minutes before this process completes 1. check the notifications 


### Step 4: Create your Storage Account (Blob)

1. Once again, browse to [https://portal.azure.com](https://portal.azure.com)
1. Click "Create Resource" from the menu on the left of the page, then search for "Storage Account" and click "Create"
<br/><img src="resources/portal_create_new_storage_account.png" width="75%" />
1. Enter in a name for the storage account (e.g. `mystorageaccount`)
1. From th "Account Kind" dropdown, select "Blob"
1. Choose your Azure Subscription
1. Choose "Use existing", and select the resource group you created in the previous section
1. Choose a region (any region is fine)
1. Click "Create" to create the Storage Account
<br/><img src="resources/portal_create_new_storage_account_settings.png" width="75%" />


### Step 6: Create a Function App solution in Visual Studio 2017

1. Launch Visual Studio and ensure your Azure Functions and Web Jobs Tools are updated to at version 15.0.31201.0
  1. Tools > Extensions and Updates > Expand the Updates node and select Visual Studio Marketplace
  1. Select Azure Functions and Web Jobs Tools and click the "Update" button
  <br/><img src="resources/vs_update_azure_tools.jpg" width="75%" />
  1. Close out and exit Visual Studio so the update can install 1. click the "Modify" button when the prompt shows
  1. Once the update completes, restart Visual Studio
1. In Visual Studio, choose File > New > Project
1. Under Visual C#, choose Cloud > Azure Functions
<br/><img src="resources/vs_new_functions_app_project.png" width="75%" />
1. Select Azure Functions v1 (.NET Framework) from the dropdown and Http Trigger as the trigger type
1. Select "Anonymous" from Access rights dropdown and click OK
<br/><img src="resources/vs_new_functions_app_project_settings.png" width="75%" />
   1. this will create a boilerplate function called `Function1` 1. we will replace this function with a real one shortly
1. Test out the function by clicking the Run/Debug button in Visual Studio
   1. You may be prompted to download and install the Azure Functions CLI tools 1. click the "Yes" button to accept
1. Use Postman or a browser to make a GET request to the function (e.g. `http://localhost:7071/api/Function1?name=Rob`) and verify the ouput `Hello, Rob`, for example
<br/><img src="resources/postman_verify_function1.png" width="75%" />


### Step 7: Publish your app to the cloud and verify

1. Right-click on your Functions project and select "Publish..."
1. Select "Azure Function App" and "Select Existing"
1. Click the Settings icon and select "Create Profile"
1. Click the "Create Profile" button
<br/><img src="resources/vs_new_publish_profile.png" width="75%" />
1. Select your Azure Subscription from the dropdown
1. Expand your the Resource Group and select the Function App you created in step 2
1. Click "OK"
<br/><img src="resources/vs_create_publish_profile_select_appservice.png" width="75%" />
1. (If you plan to attach a remote debugger)
   1. Click "Manage Profile Settings"
   <br/><img src="resources/vs_publish_profile_settings_link.png" width="75%" />
   1. Change the Configuration to "Debug" and click "Save"
   <br/><img src="resources/vs_publish_profile_settings.png" width="50%" />
1. Click the "Publish" button
   1. If you get a warning indicating the version remotely doesn't match the local version, accept by clicking "Yes"
1. Copy the site URL and verify the function is running by using Postman to send that same GET request against the remote instance (e.g. `http://myfunctionsapp.azurewebsites.net/api/Function1?name=Rob`) and verify the ouput `Hello, Rob`
<br/><img src="resources/vs_publish_profile_site_link.png" width="75%" />
1. Pat yourself on the back, you have now deployed a function


#### Step 1: Generate a Shared Access Signature for your Storage Account

> __Note:__ We use a SAS URL to grant read/write access for a specified amount of time. We'll be using this from within a function.

1. Go to your storage account 1. you can search for this or go to Resource Groups > select yours > click on your storage account in the list
1. Click on Shared Access Signature
1. Leave the default settings as is but ensure the "End Date" is set to 24hrs from now or however long you want the Functions App to have access
1. Click the "Generate SAS" button
<br/><img src="resources/portal_storage_account_generate_sas_url.png" width="85%" />
1. Copy the "Blob service SAS URL" to somewhere safe - we'll be using this to allow our function limited access to read and write to our storage account (e.g. `https://mynewstorageaccountblob.blob.core.windows.net/?sv=2017-04-17&ss=b&srt=sco&sp=rwdlac&se=2018-01-05T23:03:17Z&st=2018-01-04T23:03:17Z&spr=https&sig=HIE2i2WSliZkiQ8lAw0qjykZm1VbywZdAGbUhrWfl%2BI%3D`)

> __Note:__ This can also be done programatically to generate the URLs on demand. Check out [this code](https://github.com/rob-derosa/Hunt/blob/master/Hunt.Backend.Functions/Functions/GetStorageToken.cs#L81) as a reference.

### Step 8: Upload an image/byte[] to your blob storage account

1. Back in Visual Studio, change the method name and file name of `Function1` to `MakePrediction`
1. Also change the value of the `FunctionName` attribute to `nameof(MakePrediction)`
1. In the method signature, change the HttpTrigger methods params from `"post", "get"` to just `"post"`
1. Right-click on the project's "Dependencies" node and choose "Manage Nuget Packages..."
1. Click on the "Browse" tab and search for "Azure Storage"
1. Select the "WindowsAzure.Storage" and change the version dropdown to v8.3.0 and click "Install"
1. Follow the instructions in [this gist](https://gist.github.com/rob-derosa/87e59e3dac93882f29f8fd4fa246ff3d)
1. Add the missing using statements
1. In the Azure portal, locate your storage account and select "Browse blobs"
1. Click the "+ Container" button to create a new container to house the image blobs we'll upload
1. Name the container `images` and set the Public access level to __Blob (anonymous read access for blobs only)__
<br/><img src="resources/portal_storage_account_create_container.png" width="75%" />
1. Back in Visual Studio, update line 43 to incorporate both your storage SAS URL and the container name
1. Build and run the project locally
1. Verify this by using Postman to send a POST request to your local endpoint
    1. Set the method dropdown to `POST`
    1. Set your endpoint in the address bar
    1. Set the body content by selecting the "Body" tab and choose the "binary" option
    1. Choose a local image file to send
    1. Send the request and verify a URL comes back
    1. Copy and past this URL into a browser and validate the image is the one you posted
      1. Note: you can also verify in the Azure portal by clicking "Browse blobs" and selecting the "images" container
1. Publish to the cloud and verify this remotely following the steps outlined above


### Step 9: Write a document to your Cosmos SQL Database

1. Back in Visual Studio, right-click on the project's "Dependencies" node and choose "Manage Nuget Packages..."
1. Click on the "Browse" tab and search for "DocumentDB"
1. Select the "Microsoft.Azure.DocumentDB.Core" and ensure version 1.7.1 is selected and click "Install"
1. Add the MyCommonLibrary project to your solution
   1. Right-click on the solution and select Add > Existing Project... and navigate to the parent folder and select MyCommonLibrary.csproj
1. Add a reference to the MyCommonLibrary project to your Function App project
   1. This is a .NET Standard library that contains classes/models shared between the front and back ends
   1. Right click on the Dependencies node of the Functions app and select Add Reference...
   1. On the left side, choose Projects and check the box next to MyCommonLibrary
   1. Click the "OK" button
   <br/><img src="resources/vs_add_reference_common.png" width="50%" />
1. Right-click on your Functions app project and select Add > New Class... and name it "CosmosDataService.cs"
1. Replace the empty class with the code from [this gist](https://gist.github.com/rob-derosa/d38e6a7c1bdba90b101a3e9ad7b6dfb6)
1. Add the missing using statements
1. The `_databaseId` is the name of your database and the `_collectionId` is then name of the table/collection - feel free to change these
1. Check out the method `EnsureDatabaseConfigured` - it's responsible for creating your database and collection if either do not exist
1. In the Azure portal, navigate to your Cosmos DB
1. Click on Keys
1. In the Read-write Keys tab, copy the URI and paste it as the value of `_databaseUrl` in CosmosDataService.cs
1. In the Read-write Keys tab, copy the Primary Key and paste it as the value of `_databaseKey` in CosmosDataService.cs
<br/><img src="resources/portal_cosmos_keys.png" width="75%" />
1. In MakePrediction.cs, replace the code in the `try` clause with the code in [this gist](https://gist.github.com/rob-derosa/240dd977d41ebc58f633dbdf009d5e63)
1. Build and run your project locally
1. Use Postman to make the same local POST request as in the previous step
1. Verify the data was inserted by inspecting the Prediction JSON object that is returned.
   - You can also see the documents in your database by navigating to the Data Explorer from your Cosmos DB account and expanding down to your collection documents
1. Publish to the cloud and re-verify


### Step 10: Set up your own Custom Vision project and train a classifier


> __Note__: This step is optional as we are providing a ProjectID and TrainingID for you

1. Browse to http://customvision.ai and sign-in to create a new project


### Step 11: Make a prediction with Custom Vision and log the results

1. In Visual Studio, right-click on the project's "Dependencies" node and choose "Manage Nuget Packages..."
1. Click on the "Browse" tab and search for "CustomVision"
1. Select the "Microsoft.Cognitive.CustomVision.Prediction" and change the version dropdown to v1.0.0 and click "Install"
1. Select the "Microsoft.Cognitive.CustomVision.Training" and change the version dropdown to v1.0.0 and click "Install"
1. In MakePrediction.cs, replace the code within the `try` clause with the code in [this gist](https://gist.github.com/rob-derosa/07952c26d0bc7e78b974cae5ddec1485)
1. Add the missing using statements
1. You've just added a chunk of code that will now run a prediction against the image you upload and return a list of resulting tags and their probability
1. Build and run locally
1. Use Postman to make the same local POST request as in the previous step
1. Verify the data was inserted by inspecting the Prediction JSON object that is returned.
1. Publish to the cloud and re-verify


### Step 12: Connect the mobile front-end to the Functions backend

1. In another instance of Visual Studio, open the MyMobileApp.sln file sitting in the root folder - this will open a Xamarin Forms project targeting iOS and Android
1. Open the App.xaml.cs file and set your Azure Function endpoint on line 17.
1. Right-click on either the iOS Android project and select "Set as Startup Project", then click "Rebuild"
   - If you are targeting iOS, you will need to bridge your PC with a Mac - please see [this doc](linkhere) for more info
1. Select an emulator from the drop-down list and click the Start Debug button
  1. It can take several minutes to compile and begin the debugging session




https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/csharp-tutorial