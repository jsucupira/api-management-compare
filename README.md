Azure API Management Comparison Tool
===================
The purpose of this tool is to allow developers to compare Azure API Management End-Points between environments.
For example:
If you have a API management instance for development and a different API management instance for QA, this tool will help you compare both environment and help you synchronize them.

This tool is using the API Management REST services for it's functionality.  You can read the API Management REST documentation here: https://msdn.microsoft.com/en-us/library/azure/dn776326.aspx

**Note:** This is a very simple tool I created to help me maintain my environments in sync.  I did not pay any attention to the UI.  Feel free to update it.
Also there are some functionality that are missing because they are not available in API Management REST services.  These are:
> - Ability to set the Rewrite URL template for an operation
> - Ability to set the caching policy for an operation
> - Be able to add APIs to a product

Feel free to contact me if you have any questions.