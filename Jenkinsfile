pipeline {
  agent any
  stages {
    stage('NPM Install') {
      steps {
        parallel(
          "NPM Install": {
            retry(count: 5) {
              dir(path: 'MemoryClient.Web') {
                bat('npm install')
              }
              
            }
            
            
          },
          "Connect to Cluster": {
            bat("gcloud container clusters get-credentials memoryproject-dev --zone europe-west1-c --project plenary-vim-176019")
            
          }
        )
      }
    }
    stage('Webpack') {
      steps {
        dir(path: 'MemoryClient.Web') {
          bat("webpack")
        }
        
      }
    }
    stage('Build') {
      steps {
        bat("docker-compose -f docker-compose.ci.build.yml up --no-color")
      }
    }
    stage('Deploy') {
      steps {
        parallel(
          "Deploy API": {
            bat("docker build MemoryServer/obj/Docker/publish -t $imageTagApi")
            bat("gcloud docker -- push ${imageTagApi}")
            bat("sed -i.bak \"s#eu.gcr.io/plenary-vim-176019/api:1.0.0#${imageTagApi}#\" ./MemoryServer/k8s/${BRANCH_NAME}/*.yaml")
            bat("kubectl --namespace=${BRANCH_NAME} apply -f MemoryServer/k8s/${BRANCH_NAME}/")
            
          },
          "Deploy Web": {
            bat("docker build MemoryClient.Web/obj/Docker/publish -t ${imageTagWeb}")
            bat("gcloud docker -- push ${imageTagWeb}")
            bat("sed -i.bak \"s#eu.gcr.io/plenary-vim-176019/web:1.0.0#${imageTagWeb}#\" ./MemoryClient.Web/k8s/${BRANCH_NAME}/*.yaml")
            bat("kubectl --namespace=${BRANCH_NAME} apply -f MemoryClient.Web/k8s/${BRANCH_NAME}/")
            
          }
        )
      }
    }
    stage('Done') {
      steps {
        echo 'Success!'
      }
    }
  }
  environment {
    project = 'plenary-vim-176019'
    imageTagBase = "eu.gcr.io/${project}"
    entryName = "build.${BRANCH_NAME}.sh"
    imageTagApi = "$imageTagBase/api:${BRANCH_NAME}.${BUILD_ID}"
    imageTagWeb = "$imageTagBase/web:${BRANCH_NAME}.${BUILD_ID}"
  }
}