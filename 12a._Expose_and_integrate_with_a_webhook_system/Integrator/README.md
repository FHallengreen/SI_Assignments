# Integrator

## Objective 
The objective of this project is to expose a database to the internet and integrate it with a webhook system. This will allow us to send data from the database to a webhook endpoint whenever there is a change in the database. This can be useful for various purposes, such as sending notifications, updating other systems, or triggering workflows.

## Steps
I followed the steps from following the documentation provided by the exposee. https://github.com/DanielStarckLorenzen/SystemIntegrationMandatory2/blob/main/12a/exposee/README.md

1. I created a webhook endpoint using webhook.site. This is a free service that allows you to create a temporary webhook endpoint for testing purposes. The URL for the webhook endpoint is: https://webhook
2. I made a python script that registers the webhook `register_webhook.py`.
3. Created a virtual environment using py -m venv venv and activated it using venv\Scripts\activate.
4. Installed the required packages using pip install -r requirements.txt.
5. Ran the script using python register_webhook.py.
6. I opened swagger and tested the webhook using the ping. 
7. Afterwards i triggered the event with another endpoint to see if payment.processed events were displayed in the webhook.site.

Following screenshots show the flow.

 ![Screenshot of webhook.sites](/12a._Expose_and_integrate_with_a_webhook_system/Integrator/screenshots/1.png "Title")
 ![Screenshot of python script running](/12a._Expose_and_integrate_with_a_webhook_system/Integrator/screenshots/2.png "Title")
 ![Screenshot of ping test in Swagger](/12a._Expose_and_integrate_with_a_webhook_system/Integrator/screenshots/3.png "Title")
 ![Screenshot of webhook.sites showing the event](/12a._Expose_and_integrate_with_a_webhook_system/Integrator/screenshots/4.png "Title")
 ![Screenshot of swagger triggering an event](/12a._Expose_and_integrate_with_a_webhook_system/Integrator/screenshots/5.png "Title")
 ![Screenshot of webhook.sites displaying the event](/12a._Expose_and_integrate_with_a_webhook_system/Integrator/screenshots/6.png "Title")
