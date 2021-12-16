#!/usr/bin/env python3
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

import os

class DefaultConfig:
    """ Bot Configuration """

    PORT = 3978
    APP_ID = os.environ.get("MicrosoftAppId", "")
    APP_PASSWORD = os.environ.get("MicrosoftAppPassword", "")
    ACCOUNT_SID = os.environ.get("sid","")
    AUTH_TOKEN = os.environ.get("auth_token","")
    PHONE_NUMER = os.environ.get("phone_number","whatsapp:")
    
