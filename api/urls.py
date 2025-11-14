from django.urls import path
from . import views

urlpatterns = [
    path("", views.health_check),
    path("upload/exam", views.upload_exam,),
    path("download/exam", views.download_exam),
    path("saerch", views.get_exams)
]