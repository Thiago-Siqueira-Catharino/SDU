from django.shortcuts import render
from django.http import JsonResponse

# Create your views here.
def health_check():
    return JsonResponse({"Success":"All systems normal"}, status=200)