from django.http import JsonResponse
from django.shortcuts import render
from . import utils as u
from . import models
# Create your views here.
def health_check(request):
    return JsonResponse({"Success":"All systems normal"}, status=200)

def upload_file(request):
    if request.method != 'POST':
        return JsonResponse({"Error":"Invalid request method"}, status = 400)
    
    file = request.FILES.get("file")

    if not file:
        return JsonResponse({"Error":"No file was uploaded"}, status = 400)
    
    try:
        path = u.upload_s3(file_obj=file)
    except Exception as e:
        return JsonResponse(
            {
                "Failed":"Something went wrong while uploading the file",
                "Error":str(e),
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
        "Success":"File saved successfully"
    }, status=200)
    