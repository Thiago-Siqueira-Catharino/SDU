from django.urls import path
from . import views

urlpatterns = [
    path("", views.health_check),

    #exam related endpoints
    path("search/exam", views.get_exams),
    path("upload/exam", views.upload_exam,),
    path("download/exam", views.download_exam),

    #diagnosis related endpoints
    path("search/diagnosis", views.get_diagnoses),
    path("upload/diagnosis", views.upload_diagnosis),
    path("download/diagnosis", views.download_diagnosis),
]