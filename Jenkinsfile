pipeline {
  agent any
  stages {
    stage('NPM Install') {
      steps {
        parallel(
          "NPM Install": {
            retry(count: 5) {
              dir(path: 'MemoryClient.Web') {
                bat(script: 'npm install', returnStatus: true, returnStdout: true)
              }
              
            }
            
            
          },
          "Connect to Cluster": {
            bat(script: 'gcloud container clusters get-credentials memoryproject-dev --zone europe-west1-c --project plenary-vim-176019', returnStatus: true, returnStdout: true)
            
          }
        )
      }
    }
    stage('Webpack') {
      steps {
        bat(script: 'webpack', returnStatus: true, returnStdout: true)
      }
    }
    stage('Build') {
      steps {
        bat(script: 'docker-compose -f docker-compose.ci.build.yml up', returnStatus: true, returnStdout: true)
      }
    }
    stage('Deploy') {
      steps {
        parallel(
          "Deploy API": {
            bat(script: 'docker build MemoryServer/obj/Docker/publish -t $imageTagApi', returnStatus: true, returnStdout: true)
            bat(script: 'gcloud docker -- push ${imageTagApi}', returnStatus: true, returnStdout: true)
            bat(script: 'sed -i.bak "s#eu.gcr.io/plenary-vim-176019/api:1.0.0#${imageTagApi}#" ./MemoryServer/k8s/${BRANCH_NAME}/*.yaml', returnStatus: true, returnStdout: true)
            bat(script: 'kubectl --namespace=${BRANCH_NAME} apply -f MemoryServer/k8s/${BRANCH_NAME}/', returnStatus: true, returnStdout: true)
            
          },
          "Deploy Web": {
            bat(script: 'docker build MemoryClient.Web/obj/Docker/publish -t ${imageTagWeb}', returnStatus: true, returnStdout: true)
            bat(script: 'gcloud docker -- push ${imageTagWeb}', returnStatus: true, returnStdout: true)
            bat(script: 'sed -i.bak "s#eu.gcr.io/plenary-vim-176019/web:1.0.0#${imageTagWeb}#" ./MemoryClient.Web/k8s/${BRANCH_NAME}/*.yaml', returnStatus: true, returnStdout: true)
            bat(script: 'kubectl --namespace=${BRANCH_NAME} apply -f MemoryClient.Web/k8s/${BRANCH_NAME}/', returnStatus: true, returnStdout: true)
            
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