from django.conf import settings
from botocore.config import Config
import boto3, magic, uuid, os

ALLOWED_MIMES = {"image/png", "image/jpeg", "application/pdf"}

def upload_s3(file_obj):
    s3 = boto3.client(
        's3',
        aws_access_key_id = settings.AWS_ACCESS_KEY_ID,
        aws_secret_access_key = settings.AWS_SECRET_ACCESS_KEY,
        region_name = settings.REGION_NAME
    )

    head = file_obj.read(2048)
    mime = magic.from_buffer(head, mime=True)
    file_obj.seek(0)
    if mime not in ALLOWED_MIMES:
        raise ValueError(f"Tipo de arquivo não permitido: {mime}")

    ext = {
        "image/png":".png",
        "image/jpeg":".jpg",
        "application/pdf":".pdf",
    }.get(mime, "")

    uid = uuid.uuid4().hex
    path = f"uploads/{uid}{ext}"

    s3.upload_fileobj(
        file_obj,
        settings.BUCKET_NAME,
        path,
        ExtraArgs = {"ACL":"private", "ContentType":mime},
    )

    return path

def download_link(path:str):
    s3 = boto3.client(
        's3',
        aws_access_key_id = settings.AWS_ACCESS_KEY_ID,
        aws_secret_access_key = settings.AWS_SECRET_ACCESS_KEY,
        region_name = settings.REGION_NAME,
        config=Config(signature_version='s3v4')
    )

    response = s3.generate_presigned_url(
        'get_object',
        Params = {
            'Bucket':settings.BUCKET_NAME,
            'Key': path,
            'ResponseContentDisposition': f'attachment; filename="{path.split('/')[-1]}"',
        }, ExpiresIn=30,
    )

    return response
