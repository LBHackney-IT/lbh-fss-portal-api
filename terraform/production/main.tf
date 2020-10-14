# INSTRUCTIONS:
# 1) ENSURE YOU POPULATE THE LOCALS
# 2) ENSURE YOU REPLACE ALL INPUT PARAMETERS, THAT CURRENTLY STATE 'ENTER VALUE', WITH VALID VALUES
# 3) YOUR CODE WOULD NOT COMPILE IF STEP NUMBER 2 IS NOT PERFORMED!
# 4) ENSURE YOU CREATE A BUCKET FOR YOUR STATE FILE AND YOU ADD THE NAME BELOW - MAINTAINING THE STATE OF THE INFRASTRUCTURE YOU CREATE IS ESSENTIAL - FOR APIS, THE BUCKETS ALREADY EXIST
# 5) THE VALUES OF THE COMMON COMPONENTS THAT YOU WILL NEED ARE PROVIDED IN THE COMMENTS
# 6) IF ADDITIONAL RESOURCES ARE REQUIRED BY YOUR API, ADD THEM TO THIS FILE
# 7) ENSURE THIS FILE IS PLACED WITHIN A 'terraform' FOLDER LOCATED AT THE ROOT PROJECT DIRECTORY

provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
    application_name = "fss portal api"
    parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-production-apis"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/fss-portal-api/state"
  }
}

resource "aws_cognito_user_pool" "fss_pool" {
    name = "fss_pool"
    username_attributes = ["email"]

    email_configuration {
        from_email_address = "Find Support Services <fss@hackney.gov.uk>"
        reply_to_email_address = "fss@hackney.gov.uk"
        email_sending_account  = "DEVELOPER"
        source_arn             = "arn:aws:ses:eu-west-1:153306643385:identity/fss@hackney.gov.uk"
    }
    auto_verified_attributes   = ["email"]
    admin_create_user_config {
        allow_admin_create_user_only = false
        invite_message_template {
            email_message = "{username} Thank you for registering for Find support services. Here is your temporary password to access the administration system.<p>Password: {####}</p> Note that the password is case sensitive <p><b>Questions?</b></p>If you have any questions, please contact fss@hackney.gov.uk"
            email_subject = "Hackney & City Find Support Services - temporary password"
            sms_message   = "Your username is {username} and temporary password is {####}. "
        }
    }

    password_policy {
        minimum_length                   = 8
        require_lowercase                = true
        require_numbers                  = true
        require_symbols                  = true
        require_uppercase                = true
        temporary_password_validity_days = 7
    }

    verification_message_template {
        default_email_option = "CONFIRM_WITH_CODE"
        email_message        = "You have requested to join Find support services. Here’s your verification code: <p>{####}</p> To verify your email, you need to copy and paste the verification code into the Find support services website, as requested. <p><b>Questions?</b></p>If you have any questions, please contact fss@hackney.gov.uk"
        email_subject        = "Hackney & City Find Support Services - email verification"
        sms_message          = "Your verification code is {####}. "
    }

    tags = {
        Environment = "production"
        Terraform   = true
    }
}

resource "aws_cognito_user_pool_client" "fss_pool_client" {
    name = "fss_pool_client"
    user_pool_id = aws_cognito_user_pool.fss_pool.id
    explicit_auth_flows = ["ALLOW_ADMIN_USER_PASSWORD_AUTH", "ALLOW_USER_PASSWORD_AUTH", "ALLOW_REFRESH_TOKEN_AUTH", "ALLOW_USER_SRP_AUTH"]
}

resource "aws_cognito_user_pool_domain" "fss_domain_production" {
    domain       = "fss-pool-domain-production"
    user_pool_id = aws_cognito_user_pool.fss_pool.id
}

