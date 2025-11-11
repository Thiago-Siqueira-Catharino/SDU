from django.urls import path
from . import views

urlpatterns = [
    path("", views.health_check),
    path("upload", views.upload_file),
]