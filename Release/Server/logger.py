import logging

# log_dir = './'
# log_prefix = 'portfolio'

class Log:
    def __init__(self, log_dir, log_prefix):
        self.log_dir = log_dir
        self.log_prefix = log_prefix
        self.log_path = self.log_dir + '/' + self.log_prefix + '.log'

        self.logger = logging.getLogger(self.log_prefix)
        self.logger.setLevel(logging.DEBUG)

        self.formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
        self.fh = logging.FileHandler(self.log_path)
        self.fh.setLevel(logging.DEBUG)
        self.fh.setFormatter(self.formatter)
        self.logger.addHandler(self.fh)