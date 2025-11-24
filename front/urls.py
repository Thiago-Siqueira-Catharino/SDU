from django.urls import path
from . import views

urlpatterns = [
    path("", views.render_front),
    path("login", views.login),
    path("check_login", views.check_login)
]