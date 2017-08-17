./build.debug.ps1
gcloud container clusters get-credentials memoryproject-dev --zone europe-west1-c --project plenary-vim-176019
kubectl patch deployment memoryserver -p "{\"spec\":{\"template\":{\"metadata\":{\"labels\":{\"date\":\"`date +'%s'`\"}}}}}"
kubectl patch deployment memoryclient-web -p "{\"spec\":{\"template\":{\"metadata\":{\"labels\":{\"date\":\"`date +'%s'`\"}}}}}"