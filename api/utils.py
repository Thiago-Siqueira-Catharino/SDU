from django.conf import settings
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
    path = "uploads/{uid}{ext}"

    s3.upload_fileobj(
        file_obj,
        settings.BUCKET_NAME,
        path,
        ExtraArgs = {"ACL":"private", "ContentType":mime},
    )

    return path