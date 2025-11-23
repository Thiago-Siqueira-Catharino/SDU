from django.contrib.auth import authenticate, login
from django.http import JsonResponse
from django.shortcuts import render
from api import utils as u

# Create your views here.
def render_front(request):
    return render(request, 'index.html')

def login(request):
    request_error = u.handle_request_method(request, 'POST')
    if request_error:
        return request_error
    
    values = {}
    params = ['username', 'password']
    for param in params:
        error = u.verify_param()
        if error:
            return error
        else:
            values[param] = request.POST.get(param)

    username = values['username']
    password = values['password']

    user = authenticate(request, username=username, password=password)

    if not user:
        return JsonResponse({
            "status":"error",
            "message":"user not found or wrong credentials"
        }, status=400)
        
    login(request, user)
    return JsonResponse({
        "status":"success",
        "message":"user logged in successfully"
    }, status=200)

def check_login(request):
    request_error = u.verify_param(request, 'GET')
    if request_error:
        return request_error
    
    if request.user.is_authenticated:
        return JsonResponse({
            "status":"success",
            "message":"user already logged in"
        }, status=200)
    
    return JsonResponse({
        "status":"error",
        "message":"user not logged in yet"
    }, status=401)