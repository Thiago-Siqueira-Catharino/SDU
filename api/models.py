from django.utils import timezone
from django.db import models

class Exame(models.Model):
    cpf = models.CharField(max_length=11, blank=False, null=True)
    path = models.CharField(max_length=255, blank=False, null=True)
    tipo = models.CharField(max_length=255, blank=False, null=True)
    data = models.DateTimeField(auto_now=True)

class Diagnostico(models.Model):
    cpf = models.CharField(max_length=11, blank=False, null=True)
    cid = models.CharField(max_length=4)
    path = models.CharField(max_length=255, blank=False, null=True)
    exames = models.ManyToManyField(Exame)

class Tracker(models.Model):
    cid = models.CharField(max_length=4, unique=True)
    counter = models.PositiveIntegerField(default=1)

