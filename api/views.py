from django.views.decorators.csrf import csrf_exempt
from django.http import JsonResponse
from django.shortcuts import render
from django.db.models import F
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
    #Request method validation
    error = u.handle_request_method(request, 'POST')
    if error: return error
    
    #Text params validation
    values = {}
    params = ['cpf', 'tipo']
    for p in params:
        error = u.verify_param(request, 'POST', p)
        if error:
            return error
        else:
            values[p] = request.POST.get(p)
    
    #Text params extraction
    cpf = values['cpf']
    tipo = values['tipo']

    #File extraction and validation
    file = request.FILES.get("file")
    if not file:
        return JsonResponse({
            "status":"error",
            "message":"No file was uploaded"
            }, status = 400)
    
    #File processing
    try:
        path = u.upload_s3(file_obj=file)
    except Exception as e:
        return JsonResponse({
            "status":"error",
            "message":"something went wrong while uploading the file",
            "error":str(e),
            }, status = 500)
    
    #Data recording
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
    #Request method validation
    error = u.handle_request_method(request, 'GET')
    if error: return error
    
    #Text param validation and extraction
    id_error = u.verify_param(request, 'GET', 'id')
    if id_error:
        return id_error
    id = request.GET.get("id")
    
    #BD data retriaval
    obj = models.Exame.objects.filter(id=id).first()

    #Retrieved data validation
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
    #Request method validation
    error = u.handle_request_method(request, 'GET')
    if error: return error
    
    #User auth validation
    #if not request.user.is_authenticated:
    #    return JsonResponse({
    #        "status":"error",
    #        "message":"login requeired",
    #    }, status=401)
    
    #Text param validation
    cpf_error = u.verify_param(request, 'GET', 'cpf')
    if cpf_error: 
        return cpf_error
    cpf = request.GET.get("cpf")

    #BD data retrieval
    exames = models.Exame.objects.filter(cpf=cpf).values('id', 'cpf', 'tipo', 'data')

    #Retrieved data validation
    if not list(exames):
        return JsonResponse({
            "status":"error",
            "message":"no matching objects found in database",
            }, status=200)
    
    return JsonResponse({
        "status":"success", 
        "message":"Valid objects found in database",
        "exames":list(exames)
        })

@csrf_exempt
def upload_diagnosis(request):
    #Request method validation
    error = u.handle_request_method(request, 'POST')
    if error: return error

    #text params validation
    values = {}
    params = ['cpf', 'cid', 'exames']
    for p in params:
        error = u.verify_param(request, 'POST', p)
        if error: 
            return error
        else:
            values[p] = request.POST.get(p)

    #File validation and extraction
    file = request.FILES.get("file")
    if not file:
        return JsonResponse({
            "status":"error",
            "message":"No file was uploaded"
            }, status = 400)
    

    #Validated text params extraction
    cid = values['cid'].upper()
    cpf = values['cpf']
    exames_raw = values['exames']

    #Exames param processing
    try:
        exames_list = json.loads(exames_raw)
    except Exception as e:
        return JsonResponse({
            "status":"error",
            "message":"malformed param: exames",
            "error":str(e),
            }, status = 400)
    
    exames_objt = []
    for id in exames_list:
        obj = models.Exame.objects.filter(id=id).first()
        if not obj:
            return JsonResponse({
            "status":"error",
            "message":f"Exam object with id = {id} not found",
            }, status = 400)
        
        exames_objt.append(obj)
    
    #File processing
    try:
        path = u.upload_s3(file_obj=file)
    except Exception as e:
        return JsonResponse({
            "status":"error",
            "message":"something went wrong while uploading the file",
            "error":str(e),
            }, status = 500)
        
    #Data recording and updating
    new_file = models.Diagnostico(
        cpf = cpf,
        cid = cid,
        path = path,
    ).save()
    new_file.exames.set(exames_objt)
    new_file.save()

    try: 
        tracker, created = models.Tracker.objects.get_or_create(
            cid = cid,
            defaults={"counter":1}
        )

        if not created:
            models.Tracker.objects.filter(cid=cid).update(
                counter = F('counter') + 1
            )
            tracker.refresh_from_db()
    except Exception as e:
        return JsonResponse({
            "status":"error",
            "message":"Unable to update tracker",
            "error": str(e)            
        }, status = 500)

    return JsonResponse({
        "status":"success",
        "message":"file saved successfully"
        }, status = 200)

def download_diagnosis(request):
    #Request method validation
    error = u.handle_request_method(request, 'GET')
    if error: return error
    
    #Text param validation and extraction
    id_error = u.verify_param(request, 'GET', 'id')
    if id_error:
        return id_error
    id = request.GET.get("id")
    
    #BD data retriaval
    obj = models.Diagnostico.objects.filter(id=id).first()

    #Retrieved data validation
    if not obj:
        return JsonResponse({
            "status":"error",
            "message":f"Diagnosis object with id = {id} not found",
            }, status = 404)
    
    return JsonResponse({
        "status":"success",
        "message":f"found object with id {id}",
        "url":u.download_link(obj.path)
        }, status=200)

def get_diagnoses(request):
    #Request method validation
    error = u.handle_request_method(request, 'GET')
    if error: return error
    
    #User auth validation
    if not request.user.is_authenticated:
        return JsonResponse({
            "status":"error",
            "message":"login requeired",
        }, status=401)
    
    #Text param validation
    cpf_error = u.verify_param(request, 'GET', 'cpf')
    if cpf_error: 
        return cpf_error
    cpf = request.GET.get("cpf")

    #BD data retrieval
    diagnosticos = models.Diagnostico.objects.filter(cpf=cpf).values('id', 'cpf', 'tipo', 'data')

    #Retrieved data validation
    if not list(diagnosticos):
        return JsonResponse({
            "status":"error",
            "message":"no matching objects found in database",
            }, status=404)
    
    return JsonResponse({
        "status":"success", 
        "message":"Valid objects found in database",
        "diagnosticos":list(diagnosticos)
        })