#before running, make sure you have an Azure account

Install-Module -Name Az -AllowClobber
$creds = Get-Credential
Connect-AzAccount -Credential $creds
Update-Module -Name Az

#change to your Github repo if you want to deploy your own version of this project
#$gitrepo="https://github.com/ThomasTwiton/cs_seniorproject.git"
#$gittoken="c2176c3c87f83d3ec9a23c1d2a9d031e4697ba3b"
$webappname="pluggedin$(Get-Random)"
$location="CentralUs"

# Create a resource group.
New-AzResourceGroup -Name pluggedResourceGroup -Location $location

# Create an App Service plan in Free tier.
New-AzAppServicePlan -Name $webappname -Location $location -ResourceGroupName pluggedResourceGroup -Tier Free

# Create a web app.
New-AzWebApp -Name $webappname -Location $location -AppServicePlan $webappname -ResourceGroupName pluggedResourceGroup

#ABOVE COMPLETE, CREATES A BLANK AZURE SITE
#TO FINISH PROCESS, IN .NET, RIGHT CLICK PROJECT AND HIT PUBLISH
#IN DEVELOPMENT: USE GITHUB MASTER BRANCH AS SOURCE CODE FOR DEPLOYMENT
# SET GitHub
$PropertiesObject = @{
	token = $gittoken;
}
#Set-AzResource -PropertyObject $PropertiesObject `
-ResourceId /providers/Microsoft.Web/sourcecontrols/GitHub -ApiVersion 2015-08-01 -Force

# Configure GitHub deployment from your GitHub repo and deploy once.
#$PropertiesObject = @{
#    repoUrl = "$gitrepo";
#    branch = "master";
#}
#Set-AzResource -PropertyObject $PropertiesObject -ResourceGroupName pluggedResourceGroup `
#-ResourceType Microsoft.Web/sites/sourcecontrols -ResourceName $webappname/web `
#-ApiVersion 2015-08-01 -Force


#run the below command to delete all related resources from Azure
#Remove-AzResourceGroup -Name myResourceGroup -Force
