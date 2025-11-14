from django.urls import path
from . import views

urlpatterns = [
    path("", views.health_check),
    path("upload/exam", views.upload_exam,),
    path("download/exam", views.download_exam),
    path("search/exam", views.get_exams),
    path("upload/diagnostic", views.upload_diagnosis),
]