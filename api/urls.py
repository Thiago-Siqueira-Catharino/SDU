from django.urls import path
from . import views

urlpatterns = [
    path("", views.health_check),
    path("upload", views.upload_file),
    path("download", views.download_file),
    path("saerch", views.get_exams)
]