from django.views.decorators.csrf import csrf_exempt
from django.http import JsonResponse
from django.shortcuts import render
from . import utils as u
from . import models
import re, json
# Create your views here.
def health_check(request):
    return JsonResponse({
        "status":"success",
        "message":"All systems normal",
        }, status=200)

@csrf_exempt
def upload_exam(request):
    error = u.handle_request_method(request, 'POST')
    if error: return error
    
    file = request.FILES.get("file")
    if not file:
        return JsonResponse({
            "status":"error",
            "message":"No file was uploaded"
            }, status = 400)
    
    try:
        path = u.upload_s3(file_obj=file)
    except Exception as e:
        return JsonResponse({
            "status":"error",
            "message":"something went wrong while uploading the file",
            "error":str(e),
            }, status = 500)
    
    cpf = request.POST.get("cpf")
    tipo = request.POST.get("tipo")
    
    new_file = models.Exame(
        cpf = cpf,
        path = path,
        tipo = tipo
    )
    
    new_file.save()
    return JsonResponse({
        "status":"success",
        "message":"file saved successfully"
        }, status=200)
    
def download_exam(request):
    error = u.handle_request_method(request, 'GET')
    if error: return error
    
    id = request.GET.get("id")

    if not id:
        return JsonResponse({
            "status":"error",
            "message":"missing param: id"
            }, status = 400)
    
    obj = models.Exame.objects.filter(id=id).first()

    if not obj:
        return JsonResponse({
            "status":"error",
            "message":f"Exam object with id = {id} not found",
            }, status = 404)
    
    return JsonResponse({
        "status":"success",
        "message":f"found object with id {id}",
        "url":u.download_link(obj.path)
        }, status=200)

def get_exams(request):
    error = u.handle_request_method(request, 'POST')
    if error: return error
    
    if not request.user.is_authenticated:
        return JsonResponse({
            "status":"error",
            "message":"login requeired",
        }, status=401)
    
    cpf = request.GET.get("cpf")
    if not cpf:
        return JsonResponse({
            "status":"error",
            "message":"missing param: cpf"
            }, status=400)
    
    exames = models.Exame.objects.filter(cpf=cpf).values('id', 'cpf', 'tipo', 'data')

    if not list(exames):
        return JsonResponse({
            "status":"error",
            "message":"no matching objects found in database",
            }, status=404)
    
    return JsonResponse({
        "status":"success", 
        "message":"Valid objects found in database",
        "exames":list(exames)
        })

@csrf_exempt
def upload_diagnosis(request):
    error = u.handle_request_method(request, 'POST')
    if error: return error

    file = request.FILES.get("file")
    if not file:
        return JsonResponse({
            "status":"error",
            "message":"No file was uploaded"
            }, status = 400)
    
    try:
        path = u.upload_s3(file_obj=file)
    except Exception as e:
        return JsonResponse({
            "status":"error",
            "message":"something went wrong while uploading the file",
            "error":str(e),
            }, status = 500)
    
    cpf = request.POST.get("cpf")
    cid = request.POST.get("cid")

    exames_raw = request.POST.get("exames")
    if not exames_raw:
        return JsonResponse({
            "status":"error",
            "message":"missing param: exames",
            }, status = 400)

    try:
        exames_list = json.loads(exames_raw)
    except Exception as e:
        return JsonResponse({
            "status":"error",
            "message":"malformed param: exames",
            "error":str(e),
            }, status = 400)
    
    exames_objt = []
    for exame in exames_list:
        obj = models.Exame.objects.filter(id=exame["id"]).first()
        if not obj:
            return JsonResponse({
            "status":"error",
            "message":f"Exam object with id = {exame} not found",
            }, status = 400)
        
        exames_objt.append(obj)
        
    new_file = models.Diagnostico(
        cpf = cpf,
        cid = cid,
        path = path,
    )
    new_file.save()
    
    new_file.exames.set(exames_objt)
    new_file.save()

    return JsonResponse({
        "status":"success",
        "message":"file saved successfully"
        }, status=200)